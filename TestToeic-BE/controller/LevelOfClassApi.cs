using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class LevelOfClassApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LevelOfClassApi(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public ActionResult Get(string? userId)
    {
        if (userId == null)
        {
            var level = _context.LevelOfClasses
                .Where(a => !a.IsDelete && a.IsActive)
                    .Select(
                l => new LevelOfClassDto
                {
                    LevelOfClassId = l.LevelOfClassId,
                    LeveName = l.LevelName,
                    ClassCount = _context.Classes.Where(c => !c.IsDelete && c.IsActive)
                        .Count(c => c.LevelOfClassId == l.LevelOfClassId
                        ),
                    IsDelete = l.IsDelete,
                    IsActive = l.IsActive
                }
                ).ToList();
            return Ok(level);
        }

        var existUser = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (existUser == null)
        {
            return NotFound("Không tìm thấy người dùng");
        }

        var classOfUser = _context.MemberOfClasses
            .Where(m => m.ApplicationUserId == userId)
            .Select(s => s.classRef.LevelOfClassId).ToList();
        if (classOfUser.Any())
        {
            var levelOfUser = _context.LevelOfClasses
                .Where(l => classOfUser.Contains(l.LevelOfClassId))
                .Select(
                    l => new LevelOfClassDto
                    {
                        LevelOfClassId = l.LevelOfClassId,
                        LeveName = l.LevelName,
                        ClassCount = _context.MemberOfClasses
                            .Count(m => m.ApplicationUserId == userId 
                                        && m.classRef.LevelOfClassId == l.LevelOfClassId
                                        && !m.classRef.IsDelete && m.classRef.IsActive),
                        IsDelete = l.IsDelete,
                        IsActive = l.IsActive
                    }
                ).ToList();
            return Ok(levelOfUser);
        }
        
        return Ok(new{Mesage ="Bạn chưa tham gia khóa học nào"});
    }
}