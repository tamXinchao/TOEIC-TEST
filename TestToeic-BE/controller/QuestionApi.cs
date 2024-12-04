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

        // Chuyển đổi câu hỏi sang DTO
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

        // Nếu `id` có giá trị, lọc theo `TestId`
        if (id != null)
        {
            var filteredQuestions = _context.PointOfQuestions
                .Where(q => q.QuestionId == id)
                .Include(q => q.question) // Bao gồm câu trả lời cho mỗi câu hỏi
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
public ActionResult<Answer> Post(List<QuestionDto> questionDtos, int testId)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var groupedQuestions = questionDtos.GroupBy(q => q.GroupOfQuestion);
    float totalPointsOfNewQuestions = 0;
    var testPoint = _context.Tests.AsNoTracking()
        .Where(t => t.TestId == testId)
        .Select(t => t.PointOfTest) 
        .FirstOrDefault(); 
    var existingPoints = _context.PointOfQuestions
        .Where(q => q.TestId == testId)
        .ToList(); 
    List<PointOfQuestion> listPointToAverage = new List<PointOfQuestion>();
    var primaryQuestionIds = new Dictionary<string, int>();

    // Bắt đầu giao dịch
    using (var transaction = _context.Database.BeginTransaction())
    {
        try
        {
            foreach (var group in groupedQuestions)
            {
                var groupOfQuestion = group.Key;
                var questionsInGroup = group.ToList();

                var primaryQuestions = questionsInGroup.Where(q => q.Primary).ToList();
                var nonPrimaryQuestions = questionsInGroup.Where(q => !q.Primary).ToList();

                if (!primaryQuestions.Any() && nonPrimaryQuestions.Any())
                {
                    return BadRequest($"Nhóm {groupOfQuestion} không có câu hỏi Primary nhưng có các câu hỏi con. Vui lòng kiểm tra lại.");
                }

                // Lưu các câu hỏi Primary trước
                foreach (var questionDto in primaryQuestions)
                {
                    var newQuestion = new Question
                    {
                        Image = questionDto.Image,
                        QuestionContent = questionDto.QuestionContent,
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
                    if (questionDto.PointOfQuestion.HasValue)
                    {
                        var pointOfQuestion = new PointOfQuestion
                        {
                            Point = 0,
                            QuestionId = newQuestion.QuestionId,
                            TestId = testId,
                        };

                        newQuestion.PointOfQuestions.Add(pointOfQuestion);
                    }
                    _context.SaveChanges();

                    // Lưu QuestionId vào Dictionary sau khi đã lưu
                    var key = $"{groupOfQuestion}_{testId}";
                    // Trước khi tạo key, kiểm tra giá trị của GroupOfQuestion
                    Console.WriteLine($"GroupOfQuestion: {groupOfQuestion}, questionDto.GroupOfQuestion: {questionDto.GroupOfQuestion}");

                    primaryQuestionIds[key] = newQuestion.QuestionId;
                }

                // Lưu các câu hỏi không phải Primary
                foreach (var questionDto in nonPrimaryQuestions)
                {
                    var key = $"{groupOfQuestion}_{testId}";

                    if (primaryQuestionIds.ContainsKey(key))
                    {
                        // Gán ParentQuestionId là Id của câu hỏi Primary
                        questionDto.ParentQuestionId = primaryQuestionIds[key];
                    }
                    else
                    {
                        return BadRequest($"Không tìm thấy câu hỏi Primary cho Label {questionDto.LabelOfPrimaryQuestion} trong nhóm {groupOfQuestion}");
                    }

                    var newQuestion = new Question
                    {
                        Image = questionDto.Image,
                        QuestionContent = questionDto.QuestionContent,
                        MultipleAnswer = questionDto.MultipleAnswer,
                        Primary = false,
                        ParentQuestionId = questionDto.ParentQuestionId, // Gán ParentQuestionId
                        LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion,
                        IsDelete = false,
                        IsActive = true,
                        Answers = questionDto.Answers.Select(answer => new Answer
                        {
                            AnswerContent = answer.AnswerContent,
                            Correct = answer.Correct,
                            Explain = answer.Explain,
                        }).ToList(),
                        PointOfQuestions = new List<PointOfQuestion>()
                    };

                    if (questionDto.PointOfQuestion.HasValue)
                    {
                        var pointOfQuestion = new PointOfQuestion
                        {
                            Point = questionDto.PointOfQuestion.Value,
                            QuestionId = newQuestion.QuestionId,
                            TestId = testId,
                        };

                        newQuestion.PointOfQuestions.Add(pointOfQuestion);

                        listPointToAverage.Add(pointOfQuestion);
                        totalPointsOfNewQuestions += questionDto.PointOfQuestion.Value;
                        var totalPointsWithNew = existingPoints.Sum(p => p.Point) + totalPointsOfNewQuestions;
                        if (totalPointsWithNew > testPoint)
                        {
                            return BadRequest("Điểm của các câu hỏi đang nhiều hơn điểm của bài kiểm tra. Vui lòng kiểm tra lại");
                        }
                    }

                    _context.Questions.Add(newQuestion);
                }
            }
            foreach (var point in existingPoints)
            {
                listPointToAverage.Add(point);
            }
            var averagePointCalculator = new GetAveragePoint();
            float averagePoint = averagePointCalculator.AveragePointOfQuestion(listPointToAverage, testPoint);
            Console.WriteLine("TB: " + averagePoint);
            foreach (var point in listPointToAverage)
            {
                if (point.Point == 0)
                {
                    point.Point = averagePoint;
                }
            }

            // Nếu tất cả các câu hỏi đã được thêm thành công, commit giao dịch
            _context.SaveChanges();
            transaction.Commit();

            return Ok();
        }
        catch (Exception ex)
        {
            // Nếu có lỗi, rollback giao dịch và xóa các câu hỏi đã lưu
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