using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class MemberOfClassApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MemberOfClassApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MemberOfClassDto>> GetAll()
    {
        return _context.MemberOfClasses.Select(m => new MemberOfClassDto
        {
            ClassName = m.classRef.ClassName,
            MemberName = m.applicationUser.UserName,
            MemberId = m.applicationUser.Id,
            MemberOfClassId = m.MemberOfClassId,
            JoinDate = m.JoinDate,
            IsDelete = m.IsDelete,
            IsActive = m.IsActive
        }).ToList();
    }

    [HttpGet("getByClass")]
    public ActionResult<IEnumerable<MemberOfClassDto>> Get(int classId)
    {
        return _context.MemberOfClasses
            .Where(c => c.ClassId == classId)
            .Select(m => new MemberOfClassDto
            {
                ClassName = m.classRef.ClassName,
                MemberName = m.applicationUser.UserName,
                MemberOfClassId = m.MemberOfClassId,
                ClassId = m.ClassId,
                MemberId = m.ApplicationUserId,
                JoinDate = m.JoinDate,
                IsDelete = m.IsDelete,
                IsActive = m.IsActive
            })
            .ToList();
    }

    [HttpPost("joinClass")]
    public ActionResult JoinClass(MemberOfClassDto dto)
    {
        var newMemberOfClass = new MemberOfClass
        {
            ClassId = dto.ClassId,
            ApplicationUserId = dto.MemberId,
            JoinDate = DateTime.Now.ToUniversalTime(),
            IsDelete = false,
            IsActive = false
        };
        _context.Add(newMemberOfClass);
        _context.SaveChanges();

        return Ok(new { Message = "Bạn đã gửi yêu cầu tham gia. Vui lòng đợi duyệt!" });
    }
    
    [HttpDelete("leaveClass/{memberId}")]
    public ActionResult LeaveClass(string memberId, int classId)
    {
        var existMemberOfClass = _context.MemberOfClasses
            .FirstOrDefault(m => m.ApplicationUserId == memberId && m.ClassId == classId);
        if (existMemberOfClass == null)
        {
            return NotFound(new { Message = "Không tìm thấy lớp và người dùng cần rời" });
        }

        var existClass = _context.Classes.AsNoTracking()
            .FirstOrDefault(c => c.ClassId == classId);
        _context.Remove(existMemberOfClass);
        _context.SaveChanges();

        return Ok(new { Message = $"Bạn đã rời lớp {existClass.ClassName} thành công!" });
    }

    [HttpPut("acceptMember/{memberId}")]
    public ActionResult AcceptMember(string memberId, int classId)
    {
        // Tìm thành viên trong lớp
        var existMemberOfClass = _context.MemberOfClasses
            .FirstOrDefault(m =>
                m.ApplicationUserId == memberId && m.ClassId == classId); // Sử dụng FirstOrDefault để lấy object cụ thể

        // Tìm lớp và thành viên
        var existClass = _context.Classes.FirstOrDefault(c => c.ClassId == classId);
        var existStudent = _context.Users.FirstOrDefault(u => u.Id == memberId);

        // Kiểm tra nếu không tìm thấy thông tin
        if (existMemberOfClass == null || existClass == null || existStudent == null)
            return NotFound(new
            {
                Message =
                    $"Không tìm thấy thành viên {existStudent?.UserName ?? "không xác định"} trong lớp {existClass?.ClassName ?? "không xác định"}"
            });

        existMemberOfClass.IsActive = true;

        _context.SaveChanges();

        return Ok(new { Message = "Duyệt thành công", Member = existStudent.UserName, Class = existClass.ClassName });
    }

    [HttpDelete("rejectMember/{memberId}")]
    public ActionResult rejectMember(string memberId, int classId)
    {
        // Tìm thành viên trong lớp
        var existMemberOfClass = _context.MemberOfClasses
            .FirstOrDefault(m =>
                m.ApplicationUserId == memberId && m.ClassId == classId); // Sử dụng FirstOrDefault để lấy object cụ thể

        // Tìm lớp và thành viên
        var existClass = _context.Classes.FirstOrDefault(c => c.ClassId == classId);
        var existStudent = _context.Users.FirstOrDefault(u => u.Id == memberId);

        // Kiểm tra nếu không tìm thấy thông tin
        if (existMemberOfClass == null || existClass == null || existStudent == null)
            return NotFound(new
            {
                Message =
                    $"Không tìm thấy thành viên {existStudent?.UserName ?? "không xác định"} trong lớp {existClass?.ClassName ?? "không xác định"}"
            });

        if (existMemberOfClass.IsActive == false) _context.Remove(existMemberOfClass);

        _context.SaveChanges();

        return Ok(new { Message = "Từ chối thành công", Member = existStudent.UserName, Class = existClass.ClassName });
    }
}