using Microsoft.AspNetCore.Mvc;
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
        return _context.Schedules.Select(s => new ScheduleDto
        {
            TestId = s.test.TestId,
            DayCloseTest = s.DayCloseTest,
            DayOpenTest = s.DayOpenTest
        }).ToList();
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
            TestId = scheduleDto.TestId
        };
        _context.Schedules.Add(newSchedule);
        _context.SaveChanges();
        return Ok(new { Message = "Thêm lịch thành công" });
    }

    [HttpPut]
    public IActionResult Put(int id, ScheduleDto scheduleDto)
    {
        var existSchedule = _context.Schedules.Find(id);
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