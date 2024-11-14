using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[ApiController]
[Route("api/[controller]")]
public class StudentApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StudentApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<StudentAnswerDto>> Get()
    {
        var studentAnswer = _context.StudentPoints.AsNoTracking()
            .Include(s => s.AnswerOfStudents)
            .ThenInclude(ans => ans.Answer)
            .ThenInclude(q => q.question)
            .ThenInclude(poq => poq.PointOfQuestions)
            .Select(a => new StudentAnswerDto
            {
                Id = a.StudentPointId,
                Point = a.PointOfStudent,
                Title = a.test.classRef.ClassName,
                Completion = a.Completion,
                Duration = a.Duration,
                StudentName = a.applicationUser.UserName,
                Questions = a.AnswerOfStudents.Select(q => new QuestionDto
                {
                    QuestionId = q.question.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    Image = q.question.Image,
                    PointOfQuestion = q.PointOfAnswer,
                    Answers = new List<AnswerDto>
                    {
                        new AnswerDto
                        {
                            QuestionId = q.Answer.QuestionId,
                            AnswerContent = q.Answer.AnswerContent,
                            AnswerId = q.Answer.AnswerId,
                            Explain = q.Answer.Explain,
                            Correct = q.Answer.Correct
                        }
                    }
                    
                }).ToList()
            });
        return Ok(studentAnswer);
    }
    
}