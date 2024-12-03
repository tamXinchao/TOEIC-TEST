using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

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
    public ActionResult<IEnumerable<Test>> Get()
    {
        return _context.Tests.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Test> GetByTittle(int id)
    {
        var testDto = _context.Tests.Where(t => t.TestId == id)
            .Select(dto => new TestDto
            {
                Id = dto.TestId,
                Title = dto.classRef.ClassName,
                DateCreate = dto.TestDateCreated,
                Point = dto.PointOfTest,
                TestTime = dto.TestTime,
                UserCreate = dto.applicationUser.UserName,
                QuestionDtos = dto.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    Primary = q.question.Primary,
                    ParentQuestionId = q.question.ParentQuestionId,
                    MultipleAnswer = q.question.MultipleAnswer,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point,
                    Image = q.question.Image,
                    Answers = q.question.Answers.Select(a => new AnswerDto
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent,
                        Explain = a.Explain,
                        Correct = a.Correct
                    }).ToList()
                }).ToList()
            });
        return Ok(testDto);
    }

    [HttpGet("list")]
    public ActionResult<TestDto> GetList(int? id)
    {
        if (id != null)
        {
            var test = _context.Tests.AsNoTracking()
                .Where(t => t.TestId == id)
                .Select(t => new TestDto
                {
                    Id = t.TestId,
                    Point = t.PointOfTest,
                    UserCreate = t.applicationUser.UserName,
                    Title = t.classRef.ClassName,
                    TestTime = t.TestTime,
                    DateCreate = t.TestDateCreated,
                    QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.question.QuestionContent,
                        PointOfQuestion = q.Point
                    }).ToList()
                }).FirstOrDefault();

            if (test == null) return NotFound();

            return Ok(test);
        }

        var tests = _context.Tests.AsNoTracking().Select(t => new TestDto
        {
            Id = t.TestId,
            UserCreate = t.applicationUser.UserName,
            Title = t.classRef.ClassName,
            TestTime = t.TestTime,
            DateCreate = t.TestDateCreated,
            QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
            {
                QuestionId = q.QuestionId,
                QuestionContent = q.question.QuestionContent,
                PointOfQuestion = q.Point
            }).ToList()
        }).ToList();

        return Ok(tests);
    }

    [HttpGet("listBySchedule")]
    public ActionResult<TestDto> GetListBySchedule(string? day)
    {
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        DateTime utcDateTime;
        List<int>? schedule;
        List<TestDto>? testBySchedule;
        if (day != null)
        {
            if (DateTime.TryParse(day, out var parsedDay))
            {
                utcDateTime = new DateTime(parsedDay.Year, parsedDay.Month, parsedDay.Day, vietnamTime.Hour,
                    vietnamTime.Minute, vietnamTime.Second, DateTimeKind.Utc);
                schedule = _context.Schedules.AsNoTracking()
                    .Where(s => s.DayOpenTest <= utcDateTime && s.DayCloseTest >= utcDateTime)
                    .Select(s => s.TestId)
                    .ToList();
                testBySchedule = _context.Tests.AsNoTracking()
                    .Where(t => schedule.Contains(t.TestId) && t.IsActive == true && t.IsDelete == false)
                    .Select(t => new TestDto
                    {
                        Id = t.TestId,
                        Point = t.PointOfTest,
                        UserCreate = t.applicationUser.UserName,
                        Title = t.classRef.ClassName,
                        TestTime = t.TestTime,
                        DateCreate = t.TestDateCreated,
                        QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionContent = q.question.QuestionContent,
                            PointOfQuestion = q.Point
                        }).ToList()
                    }).ToList();

                return Ok(testBySchedule); // Return the list of TestDto for all the tests in the schedule
            }

            return BadRequest("Invalid day format"); // If the date format is invalid
        }

        utcDateTime = new DateTime(vietnamTime.Year, vietnamTime.Month, vietnamTime.Day, vietnamTime.Hour,
            vietnamTime.Minute, vietnamTime.Second, DateTimeKind.Utc);
        schedule = _context.Schedules.AsNoTracking()
            .Where(s => s.DayOpenTest <= utcDateTime && s.DayCloseTest >= utcDateTime)
            .Select(s => s.TestId)
            .ToList();
        testBySchedule = _context.Tests.AsNoTracking()
            .Where(t => schedule.Contains(t.TestId) && t.IsActive == true && t.IsDelete == false)
            .Select(t => new TestDto
            {
                Id = t.TestId,
                Point = t.PointOfTest,
                UserCreate = t.applicationUser.UserName,
                Title = t.classRef.ClassName,
                TestTime = t.TestTime,
                DateCreate = t.TestDateCreated,
                QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point
                }).ToList()
            }).ToList();

        return Ok(testBySchedule);
    }

    [HttpGet("{studentId}/listByClass")]
    public ActionResult Get(string studentId, int id)
    {
        
        var student = _context.MemberOfClasses
            .AsNoTracking()
            .Include(m => m.applicationUser)
            .FirstOrDefault(m => m.ApplicationUserId == studentId && m.ClassId == id);
        var nameofClass = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        if (student == null) return Ok(new { Message = "Bạn chưa tham gia lớp này", HasJoin = false,NameofClass = nameofClass.ClassName,IdOfClass = id });
        if (student.IsActive == false)
        {
            return Ok(new { Message = "Bạn đã gửi yêu cầu tham gia vui lòng đợi", HasJoin = false,NameofClass = nameofClass.ClassName,IdOfClass = id });
        }
        var tests = _context.TestOfClasses
            .AsNoTracking()
            .Where(t => t.ClassId == id && t.IsActive && !t.IsDelete)
            .Include(s => s.test.StickerOfTests)
            .Select(t => new TestDto
            {
                Id = t.test.TestId,
                ClassId = t.test.ClassId,
                UserCreate = t.test.applicationUser.UserName,
                Title = t.test.classRef.ClassName,
                TestTime = t.test.TestTime,
                Stickers = t.test.StickerOfTests
                    .Where(sticker => sticker.IsActive && !sticker.IsDelete)
                    .Select(sticker => new StickerDto
                    {
                        StickerId = sticker.sticker.StickerId,
                        StickerName = sticker.sticker.StickerName,
                        Note = sticker.sticker.Note
                    }).ToList(),
                IsActive = t.test.IsActive,
                IsDelete = t.test.IsDelete,
                QuestionDtos = t.test.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point
                }).ToList()
            })
            .ToList();
        if (!tests.Any())
            return Ok(new
                { StudentName = student.applicationUser.UserName, Message = "Lớp hiện chưa có bài kiểm tra nào" , HasJoin = true ,NameofClass = nameofClass.ClassName,IdOfClass = id});
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        var utcDateTime = new DateTime(vietnamTime.Year, vietnamTime.Month, vietnamTime.Day, vietnamTime.Hour,
            vietnamTime.Minute, vietnamTime.Second, DateTimeKind.Utc);

        var validTestIds = _context.Schedules
            .AsNoTracking()
            .Where(s => s.DayOpenTest <= utcDateTime && s.DayCloseTest >= utcDateTime)
            .Select(s => s.TestId)
            .ToList();
        var scheduledTests = tests.Where(t => validTestIds.Contains(t.Id)).ToList();

        return Ok(new
        {
            NameofClass = nameofClass.ClassName,
            IdOfClass = id,
            StudentName = student.applicationUser.UserName,
            HasJoin = true,
            TestOfClasses = scheduledTests.Any() ? scheduledTests : tests
        });
    }
    
     [HttpGet("listByClass")]
    public ActionResult GetByAdmin( int id)
    {
        var nameofClass = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        var tests = _context.TestOfClasses
            .AsNoTracking()
            .Where(t => t.ClassId == id && !t.IsDelete)
            .Include(s => s.test.StickerOfTests)
            .Select(t => new TestDto
            {
                Id = t.test.TestId,
                ClassId = t.test.ClassId,
                UserCreate = t.test.applicationUser.UserName,
                Title = t.test.classRef.ClassName,
                TestTime = t.test.TestTime,
                Stickers = t.test.StickerOfTests
                    .Where(sticker => sticker.IsActive && !sticker.IsDelete)
                    .Select(sticker => new StickerDto
                    {
                        StickerId = sticker.sticker.StickerId,
                        StickerName = sticker.sticker.StickerName,
                        Note = sticker.sticker.Note
                    }).ToList(),
                IsActive = t.test.IsActive,
                IsDelete = t.test.IsDelete,
                QuestionDtos = t.test.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point
                }).ToList()
            })
            .ToList();

        return Ok(new
        {
            NameofClass = nameofClass.ClassName,
            IdOfClass = id,
            HasJoin = true,
            TestOfClasses =  tests
        });
    }

    [HttpPost]
    public ActionResult CloneTest(TestDto testDto)
    {
        if (testDto == null || string.IsNullOrEmpty(testDto.UserCreate) || testDto.Id <= 0)
            return BadRequest(new { Message = "Dữ liệu đầu vào không hợp lệ!" });

        var existUser = _context.Users.FirstOrDefault(u => u.Id == testDto.UserCreate);
        if (existUser == null) return NotFound(new { Message = "Không tìm thấy người dùng!" });

        var existTest = _context.Tests.FirstOrDefault(t => t.TestId == testDto.Id);
        if (existTest == null) return NotFound(new { Message = "Không tìm thấy bài kiểm tra cần sao chép!" });

        var existPointOfQuestion = _context.PointOfQuestions
            .Where(p => p.TestId == existTest.TestId)
            .ToList();

        var existTestOfClasses = _context.TestOfClasses
            .Where(tc => tc.TestId == existTest.TestId)
            .ToList();

        var existSticker = _context.StickerOfTests
            .Where(s => s.TestId == existTest.TestId)
            .ToList();

        // Tạo bài kiểm tra mới
        var newTest = new Test
        {
            ApplicationUserId = testDto.UserCreate,
            TestDateCreated = DateTime.Now.ToUniversalTime(),
            ClassId = testDto.ClassId,
            PointOfTest = existTest.PointOfTest,
            TestTime = existTest.TestTime,
            TestOfClasses = new List<TestOfClass>(),
            StickerOfTests = new List<StickerOfTest>(),
            IsActive = true,
            IsDelete = false
        };

        _context.Tests.Add(newTest);
        _context.SaveChanges(); // Lưu trước để có TestId cho bài kiểm tra mới

        // Sao chép TestOfClasses
        foreach (var testOfClass in existTestOfClasses)
        {
            var newTestOfClass = new TestOfClass
            {
                TestId = newTest.TestId, // Gán TestId của bài kiểm tra mới
                ClassId = testOfClass.ClassId,
                IsActive = testOfClass.IsActive,
                IsDelete = testOfClass.IsDelete
            };
            newTest.TestOfClasses.Add(newTestOfClass);
            _context.TestOfClasses.Add(newTestOfClass);
        }

        // Sao chép StickerOfTests
        foreach (var sticker in existSticker)
        {
            var newSticker = new StickerOfTest
            {
                TestId = newTest.TestId, // Gán TestId của bài kiểm tra mới
                StickerId = sticker.StickerId,
                IsActive = sticker.IsActive,
                IsDelete = sticker.IsDelete
            };
            newTest.StickerOfTests.Add(newSticker);
            _context.StickerOfTests.Add(newSticker);
        }

        // Sao chép PointOfQuestions
        foreach (var point in existPointOfQuestion)
        {
            var newPointOfQuestion = new PointOfQuestion
            {
                TestId = newTest.TestId, // Gán TestId của bài kiểm tra mới
                QuestionId = point.QuestionId,
                Point = point.Point,
                IsActive = point.IsActive,
                IsDelete = point.IsDelete
            };
            _context.PointOfQuestions.Add(newPointOfQuestion);
        }

        _context.SaveChanges(); // Lưu tất cả thay đổi

        return Ok(new { Message = "Sao chép bài kiểm tra thành công!", NewTestId = newTest.TestId });
    }
}