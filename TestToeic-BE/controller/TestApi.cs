using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;
[ApiController]
[Route("api/[controller]")]
public class TestApi : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public TestApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Test>> Get()
    {
        return _context.Tests.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Test> GetByTittle(int id)
    {
        var testDto = _context.Tests.Where(t => t.TestId == id)
            .Select(dto => new TestDto
            {
                Id = dto.TestId,
                Title = dto.classRef.ClassName,
                DateCreate = dto.TestDateCreated,
                Point = dto.PointOfTest,
                TestTime = dto.TestTime,
                UserCreate = dto.applicationUser.UserName,
                Questions = dto.PointOfQuestions.Select(q => new Question
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestions = new List<PointOfQuestion>
                    {
                        new PointOfQuestion
                        {
                            Point = q.Point
                        }
                    },
                    Image = q.question.Image,
                    Answers = q.question.Answers.Select(a => new Answer
                    {
                        AnswerId = a.AnswerId,
                        QuestionId = a.QuestionId,
                        AnswerContent = a.AnswerContent,
                        Explain = a.Explain,
                        Correct = a.Correct
                    }).ToList()
                }).ToList()
            });
        return Ok(testDto);
    }
}