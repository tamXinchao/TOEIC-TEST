using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[ApiController]
[Route("api/[controller]")]
public class AnswerApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnswerApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<AnswerDto>> Get()
    {
        return Ok();
    }

    [HttpPost]
    public ActionResult<AnswerDto> Post(AnswerDto answerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newAnswer = new Answer
        {
            AnswerContent = answerDto.AnswerContent,
            Explain = answerDto.Explain,
            Correct = answerDto.Correct,
            IsDelete = false,
            IsActive = true,
            QuestionId = answerDto.QuestionId
        };

        _context.Answers.Add(newAnswer);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPut]
    public IActionResult Put(int id, AnswerDto answerDto)
    {
        var existAnswer = _context.Answers.Find(id);
        if (existAnswer == null)
        {
            return NotFound();
        }

        existAnswer.AnswerContent = answerDto.AnswerContent;
        existAnswer.Correct = answerDto.Correct;
        existAnswer.Explain = answerDto.Explain;

        _context.Answers.Update(existAnswer);
        _context.SaveChanges();
        return Ok(new {Message = "Cập nhật câu trả lời thành công!"});
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var existAnswer = _context.Answers.Find(id);
        if (existAnswer == null)
        {
            return NotFound(new{Message = "Không tìm thấy câu trả lời cần xóa"});
        }

        existAnswer.IsDelete = true;
        _context.Answers.Update(existAnswer);
        _context.SaveChanges();
        return Ok(new { Message = "Xóa câu trả lời thành công" });
    }
    
}