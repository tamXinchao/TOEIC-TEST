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
                .Include(q => q.question)    // Bao gồm câu trả lời cho mỗi câu hỏi
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
    float totalPointsOfNewQuestions = 0;
    var testPoint = _context.Tests.AsNoTracking()
        .Where(t => t.TestId == testId)
        .Select(t => t.PointOfTest) 
        .FirstOrDefault(); 
    var existingPoints = _context.PointOfQuestions
        .Where(q => q.TestId == testId)
        .ToList(); 
    List<PointOfQuestion> listPointToAverage = new List<PointOfQuestion>();

    foreach (var questionDto in questionDtos)
    {
        var newQuestion = new Question
        {
            Image = questionDto.Image,
            QuestionContent = questionDto.QuestionContent,
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
                Point = questionDto.PointOfQuestion,
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
    // Lưu toàn bộ thay đổi vào cơ sở dữ liệu
    _context.SaveChanges();
    return Ok();
}

    [HttpPut]
    public IActionResult Put(int id, QuestionDto questionDto)
    {
        var existQuestion = _context.Questions.Find(id);
        if (existQuestion == null)
        {
            return NotFound();
        }

        // Cập nhật các trường chỉ khi chúng có giá trị mới
        if (!string.IsNullOrEmpty(questionDto.QuestionContent))
        {
            existQuestion.QuestionContent = questionDto.QuestionContent;
        }
        if (questionDto.Image != null)
        {
            existQuestion.Image = questionDto.Image;
        }

        // Lưu thay đổi mà không cần cập nhật các trường khác
        _context.Questions.Update(existQuestion);
        _context.SaveChanges();

        return Ok();
    }
    [HttpDelete("{id}")]
    
    public IActionResult Delete(int id)
    {
        var existQuestion = _context.Questions.Find(id);
        if (existQuestion == null)
        {
            return NotFound();
        }
    
        existQuestion.IsDelete = true;
        _context.Questions.Update(existQuestion);
        _context.SaveChanges();
    
        return Ok(new { Message = "Đã đánh dấu câu hỏi là đã xóa thành công." });
    }
}