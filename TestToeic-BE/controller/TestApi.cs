using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity;

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
    public ActionResult<IEnumerable<Test>> get()
    {
        return _context.Tests.ToList();
    }

    [HttpGet("/title")]
    public ActionResult<Test> getByTittle(string title)
    {
        var classes = _context.Classes.Where(t => t.ClassName == title).ToList();
        return Ok(classes);
    }
}