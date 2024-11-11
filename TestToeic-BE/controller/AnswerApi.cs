using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
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
    
    
}