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
    
    [HttpGet("getByQuestion")]
    public ActionResult<IEnumerable<Question>> getByQuestion()
    {
        var questions = _context.Questions
            .Include(q => q.Answers)
            .ToList();
        var questionDto = questions.Select(q => new QuestionDto
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
        
        return Ok(questionDto);
    }
}