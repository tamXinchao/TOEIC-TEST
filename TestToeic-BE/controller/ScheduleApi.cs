using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class ScheduleApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ScheduleApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ScheduleDto>> Get()
    {
        var scheduleDtos = _context.Schedules
            .Include(s => s.test.TestOfClasses)
            .Where(a => a.IsActive == true && a.IsDelete == false)
            .Select(s => new ScheduleDto
        {
            ScheduleId = s.ScheduleId,
            TestId = s.test.TestId, // Lấy ID bài test
            TestName = s.test.TestName,
            ClassName = _context.TestOfClasses
                .Where(tc => tc.TestId == s.test.TestId)
                .Select(tc => tc.classRef.ClassName)
                .FirstOrDefault() ?? "Chưa có lớp",
            ClassId = _context.TestOfClasses
                .Where(tc => tc.TestId == s.test.TestId)
                .Select(tc => tc.classRef.ClassId)
                .FirstOrDefault(),
            DayCloseTest = s.DayCloseTest,
            DayOpenTest = s.DayOpenTest 
        }).ToList();
        return Ok(scheduleDtos);
    }

    [HttpPost]
    public ActionResult<ScheduleDto> Post(ScheduleDto scheduleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Message = "Lỗi khi nhập dữ liệu !" });
        }

        var newSchedule = new Schedule
        {
            DayOpenTest = scheduleDto.DayOpenTest,
            DayCloseTest = scheduleDto.DayCloseTest,
            TestId = scheduleDto.TestId,
            IsDelete = false,
            IsActive = true
        };
        
        _context.Schedules.Add(newSchedule);
        _context.SaveChanges();
        var scheduleDtoResponse = new ScheduleDto
        {
            ScheduleId = newSchedule.ScheduleId,
            TestId = newSchedule.TestId, // Lấy ID bài test
            TestName = _context.Tests
                .Where(t => t.TestId == newSchedule.TestId)
                .Select(t => t.TestName)
                .FirstOrDefault(),
            ClassName = _context.TestOfClasses
                .Where(tc => tc.TestId == newSchedule.TestId)
                .Select(tc => tc.classRef.ClassName)
                .FirstOrDefault() ?? "Chưa có lớp",
            ClassId = _context.TestOfClasses
                .Where(tc => tc.TestId == newSchedule.TestId)
                .Select(tc => tc.classRef.ClassId)
                .FirstOrDefault(),
            DayCloseTest = newSchedule.DayCloseTest,
            DayOpenTest = newSchedule.DayOpenTest
        };
        return Ok(new { Message = "Thêm lịch thành công" , NewSchedule = scheduleDtoResponse});
    }

    [HttpPut]
    public IActionResult Put(int id, ScheduleDto scheduleDto)
    {
        var existSchedule = _context.Schedules.FirstOrDefault(s => s.ScheduleId == id);
        if (existSchedule == null)
        {
            return NotFound(new{Message ="Không tìm thấy lịch"});
        }

        existSchedule.DayOpenTest = scheduleDto.DayOpenTest;
        existSchedule.DayCloseTest = scheduleDto.DayCloseTest;
        existSchedule.TestId = scheduleDto.TestId;

        _context.Schedules.Update(existSchedule);
        _context.SaveChanges();
        return Ok(new { Message = "Cập nhật thành công" });
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var existSchedule = _context.Schedules.Find(id);
        if (existSchedule == null)
            if (existSchedule == null)
            {
                return NotFound(new{Message ="Không tìm thấy lịch"});
            }

        existSchedule.IsDelete = true;
        _context.Schedules.Update(existSchedule);
        _context.SaveChanges();
        return Ok(new { Message = "Xóa lịch thành công" }); 
    }
}