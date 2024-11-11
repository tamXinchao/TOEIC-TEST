using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

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
    
    [HttpGet("getByQuestionByTest")]
    public ActionResult<List<QuestionDto>> GetByQuestion(int? id)
    {
        // Danh sách câu hỏi với câu trả lời liên quan
        var questions = _context.Questions
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
                AnswerContent = a.AnswerContent,
                Explain = a.Explain
            }).ToList()
        }).ToList();

        // Nếu `id` có giá trị, lọc theo `TestId`
        if (id != null)
        {
            var filteredQuestions = _context.PointOfQuestions
                .Where(q => q.QuestionId == id)  // Lọc câu hỏi theo TestId
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

    
}