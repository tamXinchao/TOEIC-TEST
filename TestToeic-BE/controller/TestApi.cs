using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                QuestionDtos = dto.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,  
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point,
                    Image = q.question.Image,
                    Answers = q.question.Answers.Select(a => new AnswerDto
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent,
                        Explain = a.Explain,
                        Correct = a.Correct
                    }).ToList()
                }).ToList()
            });
        return Ok(testDto);
    }

    [HttpGet("list")]
    public ActionResult<TestDto> GetList(int? id)
    {
        if (id != null)
        {
            var test = _context.Tests.AsNoTracking()
                .Where(t => t.TestId == id)
                .Select(t => new TestDto
                {
                    Id = t.TestId,
                    Point = t.PointOfTest,
                    UserCreate = t.applicationUser.UserName,
                    Title = t.classRef.ClassName,
                    TestTime = t.TestTime,
                    DateCreate = t.TestDateCreated,
                    QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.question.QuestionContent,
                        PointOfQuestion = q.Point
                    }).ToList(),
                }).FirstOrDefault();

            if (test == null)
            {
                return NotFound();  // Trả về 404 nếu không tìm thấy Test
            }

            return Ok(test);
        }

        // Nếu id == null, trả về tất cả bài kiểm tra
        var tests = _context.Tests.AsNoTracking().Select(t => new TestDto
        {
            Id = t.TestId,
            UserCreate = t.applicationUser.UserName,
            Title = t.classRef.ClassName,
            TestTime = t.TestTime,
            DateCreate = t.TestDateCreated,
            QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionContent = q.question.QuestionContent,
                PointOfQuestion = q.Point
            }).ToList()
        }).ToList();  // Thêm .ToList() để thực thi truy vấn

        return Ok(tests);  // Trả về danh sách tất cả bài kiểm tra
    }

}