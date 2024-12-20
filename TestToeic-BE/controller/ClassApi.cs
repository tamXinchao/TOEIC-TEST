using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class ClassApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClassApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ClassDto>> GetAll()
    {
        return _context.Classes
            .Where(c => c.IsDelete == false && c.IsActive)
            .Select(a => new ClassDto
        {
            ClassId = a.ClassId,
            ClassName = a.ClassName,
            IsDelete = a.IsDelete,
            IsActive = a.IsActive,
            MemberCount = _context.MemberOfClasses
                .Where(a => a.IsActive == true && a.IsDelete == false)
                .Count(m => m.ClassId == a.ClassId),
            MemberRequest = _context.MemberOfClasses
                .Where(a => a.IsActive == true && a.IsDelete == false)
                .Count(m => m.ClassId == a.ClassId ),
            TestCount = _context.TestOfClasses
                .Where(ab => ab.IsActive == true && ab.IsDelete == false)
                .Count(m => m.ClassId == a.ClassId )
        }).ToList();
    }
    
    [HttpGet("getByLevel/{userId}")]
    public ActionResult<IEnumerable<ClassDto>> GetAll(int levelId, string? userId)
    {
        // Retrieve classes for the user based on userId
        var existClassOfMember = _context.MemberOfClasses
            .Where(m => m.ApplicationUserId == userId)
            .ToList();

        // Check if the user is enrolled in any classes
        if (existClassOfMember == null || !existClassOfMember.Any())
        {
            return NotFound(new { Message = "Bạn chưa tham gia lớp nào" });
        }

        // Retrieve classes that match the levelId and the user's memberships
        var classes = _context.Classes
            .Where(c => !c.IsDelete && c.IsActive  && c.LevelOfClassId == levelId && 
                        _context.MemberOfClasses
                            .Any(m => m.ClassId == c.ClassId && m.ApplicationUserId == userId && m.IsDelete == false))
            .Select(a => new ClassDto
            {
                ClassId = a.ClassId,
                ClassName = a.ClassName,
                IsDelete = a.IsDelete,
                IsActive = a.IsActive,
                MemberCount = _context.MemberOfClasses
                    .Where(a => a.IsActive == true && a.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId),
                MemberRequest = _context.MemberOfClasses
                    .Where(a => a.IsActive == true && a.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId),
                TestCount = _context.TestOfClasses
                    .Where(ab => ab.IsActive == true && ab.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId)
            })
            .ToList();

        // Return the found classes
        return Ok(classes);
    }

    [HttpGet("getByLevel")]
    public ActionResult<IEnumerable<ClassDto>> GetByLevel(int levelId)
    {
        // Retrieve classes for the specified levelId
        var classes = _context.Classes
            .Where(c => c.IsDelete == false  && c.LevelOfClassId == levelId)
            .Select(a => new ClassDto
            {
                ClassId = a.ClassId,
                ClassName = a.ClassName,
                IsDelete = a.IsDelete,
                IsActive = a.IsActive,
                MemberCount = _context.MemberOfClasses
                    .Where(a => a.IsActive == true && a.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId),
                MemberRequest = _context.MemberOfClasses
                    .Where(a => a.IsActive == false && a.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId),
                TestCount = _context.TestOfClasses
                    .Where(ab => ab.IsActive == true && ab.IsDelete == false)
                    .Count(m => m.ClassId == a.ClassId)
            }).ToList();

        if (!classes.Any())
        {
            return NotFound(new { Message = "Không tìm thấy lớp học cho cấp độ này." });
        }

        return Ok(classes);
    }


    [HttpPost]
    public ActionResult<ClassDto> Post(ClassDto classDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new {Message = "Lỗi dữ liệu khi thêm lớp mới !"});
        }

        if (string.IsNullOrWhiteSpace(classDto.ClassName))
        {
            return BadRequest(new { Message = "Trường 'className' không được để trống." });
        }
        
        var newClass = new Class
        {
            ClassName = classDto.ClassName,
            IsDelete = false,
            IsActive = classDto.IsActive
        };
        
        _context.Classes.Add(newClass);
        _context.SaveChanges();
        
        return Ok(new { Message = "Thêm lớp thành công" , NewClass = newClass});
    }

    [HttpPut]
    public IActionResult Put(int id ,ClassDto classDto)
    {
        var existClass = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        if (existClass == null)
        {
            return NotFound(new { Message = "Không tìm thấy dữ liệu của lớp !" });
        }

        if (string.IsNullOrWhiteSpace(classDto.ClassName) || string.IsNullOrEmpty(classDto.ClassName))
        {
            return BadRequest(new
                { Message = "Vui lòng điền đầy đủ thông tin" });
        }
        existClass.ClassName = classDto.ClassName;
        existClass.IsActive = classDto.IsActive;
        _context.Classes.Update(existClass);
        _context.SaveChanges();
        return Ok(new { Message = "Cập nhật lớp thành công" });
    }

    [HttpDelete]
    public IActionResult DeleteClass(int id)
    {
        var existClass = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        if (existClass == null)
        {
            return NotFound(new { Message = "Không tìm thấy 'Lớp' cần xóa" });
        }

        existClass.IsDelete = true;

        _context.Classes.Update(existClass);
        _context.SaveChanges();
        return Ok(new { Message = " Xóa thành công lớp" });
    }
}