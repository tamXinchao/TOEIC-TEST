using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;
using TestToeic.utils;

namespace TestToeic.controller;

[ApiController]
[Route("api/[controller]")]
public class QuestionApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuestionApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Question>> get()
    {
        return _context.Questions.ToList();
    }

    [HttpGet("getQuestion")]
    public ActionResult<List<QuestionDto>> GetByQuestion(int? id)
    {
        // Danh sách câu hỏi với câu trả lời liên quan
        var questions = _context.Questions
            .Where(q => q.IsDelete == false)
            .Include(q => q.Answers)
            .ToList();

        var questionDtos = questions.Select(q => new QuestionDto
        {
            QuestionId = q.QuestionId,
            QuestionContent = q.QuestionContent,
            Answers = q.Answers.Select(a => new AnswerDto
            {
                AnswerId = a.AnswerId,
                QuestionId = q.QuestionId,
                AnswerContent = a.AnswerContent,
                Explain = a.Explain
            }).ToList()
        }).ToList();

        if (id != null)
        {
            var filteredQuestions = _context.PointOfQuestions
                .Where(q => q.QuestionId == id)
                .Include(q => q.question)
                .ToList();

            questionDtos = filteredQuestions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionContent = q.question.QuestionContent,
                PointOfQuestion = q.Point,
                Answers = q.question.Answers.Select(a => new AnswerDto
                {
                    AnswerId = a.AnswerId,
                    AnswerContent = a.AnswerContent,
                    Explain = a.Explain
                }).ToList()
            }).ToList();
        }

        return Ok(questionDtos);
    }

    [HttpPost]
    public ActionResult Post(List<QuestionDto> questionDtos, int testId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var groupedQuestions = questionDtos.GroupBy(q => q.GroupOfQuestion);
        float totalPointsOfNewQuestions = 0;
        var testPoint = _context.Tests.AsNoTracking()
            .Where(t => t.TestId == testId)
            .Select(t => t.PointOfTest)
            .FirstOrDefault();
        var existingPoints = _context.PointOfQuestions
            .Where(q => q.TestId == testId)
            .ToList();
        var questionsInTest = _context.PointOfQuestions.Where(p => p.TestId == testId).ToList();
        var listPointToAverage = new List<PointOfQuestion>();
        var primaryQuestionIds = new Dictionary<string, int>();
        
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {

                _context.SaveChanges();
                
                foreach (var group in groupedQuestions)
                {
                    var groupOfQuestion = group.Key;
                    var questionsInGroup = group.ToList();

                    var primaryQuestions = questionsInGroup.Where(q => q.Primary).ToList();
                    var nonPrimaryQuestions = questionsInGroup.Where(q => !q.Primary).ToList();
                    if (!primaryQuestions.Any() && nonPrimaryQuestions.Any())
                        return BadRequest(
                            $"Nhóm {groupOfQuestion} không có câu hỏi Primary nhưng có các câu hỏi con. Vui lòng kiểm tra lại.");

                    foreach (var questionDto in primaryQuestions)
                    {
                        var existQuestion =
                            _context.Questions.FirstOrDefault(q => q.QuestionId == questionDto.QuestionId);
                        if (existQuestion == null)
                        {
                            var newQuestion = new Question
                            {
                                Image = questionDto.Image,
                                QuestionContent = questionDto.QuestionContent ?? "Chưa có câu hỏi",
                                MultipleAnswer = false,
                                Primary = true,
                                ParentQuestionId = null,
                                LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion,
                                IsDelete = false,
                                IsActive = true,
                                Answers = new List<Answer>(),
                                PointOfQuestions = new List<PointOfQuestion>()
                            };
                            _context.Questions.Add(newQuestion);

                            var pointOfQuestion = new PointOfQuestion
                            {
                                Point = 0,
                                QuestionId = newQuestion.QuestionId,
                                TestId = testId
                            };

                            newQuestion.PointOfQuestions.Add(pointOfQuestion);


                            _context.SaveChanges();

                            var key = $"{groupOfQuestion}_{testId}";
                            Console.WriteLine(
                                $"GroupOfQuestion: {groupOfQuestion}, questionDto.GroupOfQuestion: {questionDto.GroupOfQuestion}");

                            primaryQuestionIds[key] = newQuestion.QuestionId;
                        }
                        else
                        {
                            existQuestion.QuestionId = questionDto.QuestionId;
                            existQuestion.QuestionContent = questionDto.QuestionContent ?? "Chưa có câu hỏi";
                            existQuestion.MultipleAnswer = questionDto.MultipleAnswer;
                            existQuestion.LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion;
                            existQuestion.Primary = true;
                            existQuestion.IsActive = questionDto.IsActive;
                            existQuestion.IsDelete = questionDto.IsDelete;
                            existQuestion.Image = questionDto.Image;
                            existQuestion.Answers = new List<Answer>();

                            _context.Questions.Update(existQuestion);
                            _context.SaveChanges();
                            var key = $"{groupOfQuestion}_{testId}";
                            Console.WriteLine(
                                $"GroupOfQuestion: {groupOfQuestion}, questionDto.GroupOfQuestion: {questionDto.GroupOfQuestion}");

                            primaryQuestionIds[key] = existQuestion.QuestionId;
                        }
                    }

                    foreach (var questionDto in nonPrimaryQuestions)
                    {
                        var key = $"{groupOfQuestion}_{testId}";
                        if (primaryQuestionIds.ContainsKey(key))
                            questionDto.ParentQuestionId = primaryQuestionIds[key];

                        else
                            return BadRequest(
                                $"Không tìm thấy câu hỏi Primary cho Label {questionDto.LabelOfPrimaryQuestion} trong nhóm {groupOfQuestion}");

                        var existNonQuestion =
                            _context.Questions
                                .Include(q => q.Answers)
                                .FirstOrDefault(q => q.QuestionId == questionDto.QuestionId);

                        if (existNonQuestion == null)
                        {
                            var newQuestion = new Question
                            {
                                Image = questionDto.Image,
                                QuestionContent = questionDto.QuestionContent ?? "Chưa có câu hỏi",
                                MultipleAnswer = questionDto.MultipleAnswer,
                                Primary = false,
                                ParentQuestionId = questionDto.ParentQuestionId,
                                LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion,
                                IsDelete = false,
                                IsActive = true,
                                Answers = questionDto.Answers?.Select(answer => new Answer
                                {
                                    AnswerContent = answer.AnswerContent,
                                    Correct = answer.Correct,
                                    Explain = answer.Explain
                                }).ToList() ?? new List<Answer>(),
                                PointOfQuestions = new List<PointOfQuestion>()
                            };

                            if (questionDto.PointOfQuestion.HasValue)
                            {
                                var pointOfQuestion = new PointOfQuestion
                                {
                                    Point = questionDto.PointOfQuestion.Value,
                                    QuestionId = newQuestion.QuestionId,
                                    TestId = testId
                                };

                                newQuestion.PointOfQuestions.Add(pointOfQuestion);

                                listPointToAverage.Add(pointOfQuestion);
                                Console.WriteLine(pointOfQuestion.Point);
                                totalPointsOfNewQuestions += questionDto.PointOfQuestion.Value;
                                if (existingPoints.Sum(p => p.Point) >= testPoint)
                                {
                                    return BadRequest(new
                                        {
                                            Message = "Điểm hiện tại của bài kiểm tra đã đạt tối đa. Hãy điều chỉnh lại để có thể tính điểm cho câu hỏi!",
                                            Suggest = "Hãy chỉnh điểm các câu hỏi về 0 để tự tính điểm cho toàn bộ câu hỏi. Hoặc điều chỉnh điểm các câu hỏi cũ thấp xuống"
                                        }
                                        );
                                }
                                var totalPointsWithNew = existingPoints.Sum(p => p.Point) + totalPointsOfNewQuestions;
                                if (totalPointsWithNew > testPoint)
                                    return BadRequest(
                                        "Điểm của các câu hỏi đang nhiều hơn điểm của bài kiểm tra. Vui lòng kiểm tra lại");
                            }
                            else
                            {
                                return BadRequest();
                            }

                            _context.Questions.Add(newQuestion);
                        }
                        else
                        {
                            existNonQuestion.QuestionId = questionDto.QuestionId;
                            existNonQuestion.QuestionContent = questionDto.QuestionContent ?? "Chưa có câu hỏi";
                            existNonQuestion.MultipleAnswer = questionDto.MultipleAnswer;
                            existNonQuestion.Primary = questionDto.Primary;
                            existNonQuestion.IsActive = questionDto.IsActive;
                            existNonQuestion.IsDelete = questionDto.IsDelete;
                            existNonQuestion.Image = questionDto.Image;

                            if (questionDto.Answers != null)
                            {
                                foreach (var answerDto in questionDto.Answers)
                                {
                                    var existAnswer =
                                        _context.Answers.FirstOrDefault(a => a.AnswerId == answerDto.AnswerId);
                                    if (existAnswer != null)
                                    {
                                        existAnswer.AnswerContent = answerDto.AnswerContent;
                                        existAnswer.Explain = answerDto.Explain;
                                        existAnswer.Correct = answerDto.Correct;
                                        existAnswer.IsActive = answerDto.IsActive;
                                        existAnswer.IsDelete = answerDto.IsDelete;
                                        _context.Answers.Update(existAnswer);
                                        Console.WriteLine("Answer");
                                    }
                                    else
                                    {
                                        var newAnswer = new Answer
                                        {
                                            AnswerContent = answerDto.AnswerContent,
                                            Explain = answerDto.Explain,
                                            Correct = answerDto.Correct,
                                            IsActive = true,
                                            IsDelete = false
                                        };
                                        existNonQuestion.Answers.Add(newAnswer);
                                    }
                                }


                                var existPointOfQuestion = _context.PointOfQuestions
                                    .FirstOrDefault(p =>
                                        p.TestId == testId && p.QuestionId == existNonQuestion.QuestionId);

                                if (existPointOfQuestion != null)
                                {
                                    if (questionDto.PointOfQuestion != null)
                                        existPointOfQuestion.Point = questionDto.PointOfQuestion;
                                    _context.PointOfQuestions.Update(existPointOfQuestion);
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    return BadRequest(new { Message = "Sai rồi" });
                                }

                                Console.WriteLine("Diem" + existPointOfQuestion.Point);
                            }
                            _context.SaveChanges();
                        }
                    }
                }

                listPointToAverage.AddRange(existingPoints);
                if (!listPointToAverage.Any())
                {
                    return BadRequest("Danh sách điểm trống hoặc không hợp lệ.");
                }

                if (listPointToAverage.Any(p => p == null))
                {
                    return BadRequest("Danh sách điểm chứa phần tử null.");
                }
                Console.WriteLine(listPointToAverage);
                var averagePointCalculator = new GetAveragePoint();
                var averagePoint = averagePointCalculator.AveragePointOfQuestion(listPointToAverage, testPoint);
                Console.WriteLine("TB: " + averagePoint);
                foreach (var point in listPointToAverage.Where(point => point.Point == 0 && !point.question.Primary))
                {
                    if (point.question.Primary)
                    {
                        point.Point = 0;
                    }
                    point.Point = averagePoint;
                }

                _context.SaveChanges();
                transaction.Commit();

                return Ok(new { Message = "Lưu thành công rồi má" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }


    [HttpPut]
    public IActionResult Put(int id, QuestionDto questionDto)
    {
        var existQuestion = _context.Questions.Find(id);
        if (existQuestion == null) return NotFound();

        // Cập nhật các trường chỉ khi chúng có giá trị mới
        if (!string.IsNullOrEmpty(questionDto.QuestionContent))
            existQuestion.QuestionContent = questionDto.QuestionContent;
        if (questionDto.Image != null) existQuestion.Image = questionDto.Image;

        // Lưu thay đổi mà không cần cập nhật các trường khác
        _context.Questions.Update(existQuestion);
        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existQuestion = _context.Questions.Find(id);
        if (existQuestion == null) return NotFound();

        existQuestion.IsDelete = true;
        _context.Questions.Update(existQuestion);
        _context.SaveChanges();

        return Ok(new { Message = "Đã đánh dấu câu hỏi là đã xóa thành công." });
    }
}