using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;

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
}