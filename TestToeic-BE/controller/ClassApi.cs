using Microsoft.AspNetCore.Mvc;
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
        return _context.Classes.Select(a => new ClassDto
        {
            ClassId = a.ClassId,
            ClassName = a.ClassName
        }).ToList();
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
            ClassName = classDto.ClassName
        };
        
        _context.Classes.Add(newClass);
        _context.SaveChanges();
        
        return Ok(new { Message = "Thêm lớp thành công" });
    }

    [HttpPut]
    public IActionResult Put(int id ,ClassDto classDto)
    {
        var existClass = _context.Classes.Find(id);
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
        _context.Classes.Update(existClass);
        _context.SaveChanges();
        return Ok(new { Message = "Cập nhật lớp thành công" });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var existClass = _context.Classes.Find(id);
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