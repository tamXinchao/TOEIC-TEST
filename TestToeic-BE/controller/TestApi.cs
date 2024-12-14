using System.Runtime.InteropServices;
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
    public ActionResult<Test> GetById(int id)
    {
        bool mutipleSection = false;
        int primaryCount = 0;
        var testDto = _context.Tests.Where(t => t.TestId == id && !t.IsDelete && t.IsActive)
            .Select(dto => new TestDto
            {
                Id = dto.TestId,
                Title = dto.TestName,
                DateCreate = dto.TestDateCreated,
                Point = dto.PointOfTest,
                TestTime = dto.TestTime,
                UserCreate = dto.applicationUser.UserName?? "Chưa có tên người tạo",
                StickersOfTests = dto.StickerOfTests
                    .Where(soc => soc.IsActive && !soc.IsDelete)
                    .Select(sticker => new StickerOfTestDto
                    {
                       StickerId = sticker.sticker.StickerId,
                       IsDelete = sticker.sticker.IsDelete,
                       IsActive = sticker.sticker.IsActive,
                       TestId = sticker.TestId,
                       StickerOfTestId = sticker.StickerOfTestId
                    }).ToList(),
                QuestionDtos = dto.PointOfQuestions
                    .Where(poq => !poq.IsDelete && poq.IsActive)
                    .Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    Primary = q.question.Primary,
                    ParentQuestionId = q.question.ParentQuestionId,
                    LabelOfPrimaryQuestion = q.question.LabelOfPrimaryQuestion,
                    MultipleAnswer = q.question.MultipleAnswer,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point,
                    Image = q.question.Image,
                    GroupOfQuestion = q.question.ParentQuestionId,
                    Answers = q.question.Answers
                        .Where(aoq => aoq.IsActive && !aoq.IsDelete)
                        .Select(a => new AnswerDto
                    {
                        AnswerId = a.AnswerId,
                        AnswerContent = a.AnswerContent,
                        Explain = a.Explain,
                        Correct = a.Correct
                    }).ToList()
                }).ToList()
            }).FirstOrDefault();
        if (testDto != null)
        {
            var questions = testDto.QuestionDtos;

            foreach (var question in questions)
            {
                if (question.Primary)
                {
                    question.GroupOfQuestion = question.QuestionId;
                    primaryCount++;
                }
            }
        }

        mutipleSection = primaryCount > 1;

        return Ok(
           new
           { MutipleSection = mutipleSection,
               PrimaryCount = primaryCount,
               TestDto = testDto
           }        
        );
    }

    [HttpGet("list")]
    public ActionResult<TestDto> GetList(string? id)
    {
        int.TryParse(id, out var parsedId);
        if (id != null)
        {
            var existUser = _context.Tests.AsNoTracking()
                .Where(u => u.TestId == parsedId || u.TestName.ToLower().Contains(id.ToLower()) ||
                            u.applicationUser.UserName.ToLower().Contains(id.ToLower()) && !u.IsDelete
                )
                .ToList();

            if (existUser.Count == 0) return NotFound($"Không tìm thấy người dùng {id}");
            var test = _context.Tests.AsNoTracking()
                .Where(t => existUser.Select(u => u.TestId).Contains(t.TestId) && !t.IsDelete )
                .Select(t => new TestDto
                {
                    Id = t.TestId,
                    Point = t.PointOfTest,
                    UserCreate = t.applicationUser.UserName ?? "Chưa có tên người tạo",
                    Title = t.TestName,
                    TestTime = t.TestTime,
                    DateCreate = t.TestDateCreated,
                    IsDelete = t.IsDelete,
                    IsActive = t.IsActive,
                    QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                    {
                        QuestionId = q.QuestionId,
                        QuestionContent = q.question.QuestionContent,
                        PointOfQuestion = q.Point
                    }).ToList()
                }).ToList();

            if (test == null) return NotFound();

            return Ok(test);
        }

        var tests = _context.Tests.AsNoTracking()
            .Where(c => !c.IsDelete)
            .Select(t => new TestDto
        {
            Id = t.TestId,
            UserCreate = t.applicationUser.UserName ?? "Chưa có người tạo",
            Title = t.TestName,
            TestTime = t.TestTime,
            DateCreate = t.TestDateCreated,
            IsDelete = t.IsDelete,
            IsActive = t.IsActive,
            Point = t.PointOfTest,
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
                    .Where(t => schedule.Contains(t.TestId) && t.IsActive && !t.IsDelete )
                    .Select(t => new TestDto
                    {
                        Id = t.TestId,
                        Point = t.PointOfTest,
                        UserCreate = t.applicationUser.UserName,
                        Title = t.TestName,
                        TestTime = t.TestTime,
                        DateCreate = t.TestDateCreated,
                        IsDelete = t.IsDelete,
                        IsActive = t.IsActive,
                        QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionContent = q.question.QuestionContent,
                            PointOfQuestion = q.Point
                        }).ToList()
                    }).ToList();

                return Ok(testBySchedule);
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
                Title = t.TestName,
                TestTime = t.TestTime,
                DateCreate = t.TestDateCreated,
                QuestionDtos = t.PointOfQuestions.Select(q => new QuestionDto
                {
                    QuestionId = q.QuestionId,
                    QuestionContent = q.question.QuestionContent,
                    PointOfQuestion = q.Point,
                    IsDelete = q.IsDelete,
                    IsActive = q.IsActive
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
        if (student == null)
            return Ok(new
            {
                Message = "Bạn chưa tham gia lớp này", HasJoin = false, NameofClass = nameofClass.ClassName,
                IdOfClass = id
            });
        if (student.IsActive == false)
            return Ok(new
            {
                Message = "Bạn đã gửi yêu cầu tham gia vui lòng đợi", HasJoin = false,
                NameofClass = nameofClass.ClassName, IdOfClass = id
            });
        var tests = _context.TestOfClasses
            .AsNoTracking()
            .Where(t => t.ClassId == id && t.IsActive && !t.IsDelete)
            .Include(s => s.test.StickerOfTests)
            .Select(t => new TestDto
            {
                Id = t.test.TestId,
                UserCreate = t.test.applicationUser.UserName,
                Title = t.test.TestName,
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
            {
                StudentName = student.applicationUser.UserName, Message = "Lớp hiện chưa có bài kiểm tra nào",
                HasJoin = true, NameofClass = nameofClass.ClassName, IdOfClass = id
            });
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
    public ActionResult GetByAdmin(int id)
    {
        var nameofClass = _context.Classes.FirstOrDefault(c => c.ClassId == id);
        var tests = _context.TestOfClasses
            .AsNoTracking()
            .Where(t => t.ClassId == id && !t.test.IsDelete)
            .Include(s => s.test.StickerOfTests)
            .Select(t => new TestDto
            {
                Id = t.test.TestId,
                Point = t.test.PointOfTest,
                UserCreate = t.test.applicationUser.UserName ?? "Chưa có tên người tạo",
                Title = t.test.TestName,
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
            TestOfClasses = tests
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
            TestName = existTest.TestName,
            PointOfTest = existTest.PointOfTest,
            TestTime = existTest.TestTime,
            TestOfClasses = new List<TestOfClass>(),
            StickerOfTests = new List<StickerOfTest>(),
            IsActive = true,
            IsDelete = false
        };

        _context.Tests.Add(newTest);
        _context.SaveChanges(); // Lưu trước để có TestId cho bài kiểm tra mới

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

        foreach (var sticker in existSticker)
        {
            var newSticker = new StickerOfTest
            {
                TestId = newTest.TestId,
                StickerId = sticker.StickerId,
                IsActive = sticker.IsActive,
                IsDelete = sticker.IsDelete
            };
            newTest.StickerOfTests.Add(newSticker);
            _context.StickerOfTests.Add(newSticker);
        }

        List<QuestionDto> questionDtos = new List<QuestionDto>();
        var primaryQuestionIds = new Dictionary<string, int>();
        foreach (var point in existPointOfQuestion)
        {
            var existingQuestion = _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefault(q => q.QuestionId == point.QuestionId);
            var pointOfQuestion = existingQuestion.PointOfQuestions
                    
                .FirstOrDefault(p => existingQuestion.QuestionId == p.QuestionId && existTest.TestId == p.TestId);
            var questionDto = new QuestionDto
            {
                QuestionId = existingQuestion.QuestionId,
                QuestionContent = existingQuestion.QuestionContent,
                MultipleAnswer = existingQuestion.MultipleAnswer,
                Primary = existingQuestion.Primary,
                ParentQuestionId = existingQuestion.ParentQuestionId,
                LabelOfPrimaryQuestion = existingQuestion.LabelOfPrimaryQuestion,
                PointOfQuestion = pointOfQuestion.Point,
                Image = existingQuestion.Image,
                IsDelete = existingQuestion.IsDelete,
                IsActive = existingQuestion.IsActive,
                Answers = existingQuestion.Answers.Select(a => new AnswerDto
                {
                    AnswerId = a.AnswerId,
                    AnswerContent = a.AnswerContent,
                    Correct = a.Correct,
                    Explain = a.Explain,
                    IsActive = a.IsActive,
                    IsDelete = a.IsDelete
                }).ToList() ?? new List<AnswerDto>() 
            };
            questionDtos.Add(questionDto);
        }
        foreach (var questionDto in questionDtos)
        {
            if (questionDto.Primary)
            {
                questionDto.GroupOfQuestion = questionDto.QuestionId;
            }
            else
            {
                questionDto.GroupOfQuestion = (int)questionDto.ParentQuestionId;
            }
        }
        var groupedQuestions = questionDtos.GroupBy(q => q.GroupOfQuestion);
          foreach (var group in groupedQuestions)
            {
                var groupOfQuestion = group.Key;
                var questionsInGroup = group.ToList();

                var primaryQuestions = questionsInGroup.Where(q => q.Primary).ToList();
                var nonPrimaryQuestions = questionsInGroup.Where(q => !q.Primary).ToList();

                if (!primaryQuestions.Any() && nonPrimaryQuestions.Any())
                {
                    return BadRequest($"Nhóm {groupOfQuestion} không có câu hỏi Primary nhưng có các câu hỏi con. Vui lòng kiểm tra lại.");
                }

                // Lưu các câu hỏi Primary trước
                foreach (var questionDto in primaryQuestions)
                {
                    var newQuestion = new Question
                    {
                        Image = questionDto.Image,
                        QuestionContent = questionDto.QuestionContent,
                        MultipleAnswer = false,
                        Primary = true,
                        ParentQuestionId = null,
                        LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion,
                        IsDelete = false,
                        IsActive = true,
                        Answers = new List<Answer>(),
                        PointOfQuestions = new List<PointOfQuestion>()
                    };
                    _context.Questions.Add(newQuestion);
                    if (questionDto.PointOfQuestion.HasValue)
                    {
                        var pointOfQuestion = new PointOfQuestion
                        {
                            Point = 0,
                            QuestionId = newQuestion.QuestionId,
                            TestId = newTest.TestId,
                        };

                        newQuestion.PointOfQuestions.Add(pointOfQuestion);
                    }
                    _context.SaveChanges();

                    // Lưu QuestionId vào Dictionary sau khi đã lưu
                    var key = $"{groupOfQuestion}_{newTest.TestId}";
                    // Trước khi tạo key, kiểm tra giá trị của GroupOfQuestion
                    Console.WriteLine($"GroupOfQuestion: {groupOfQuestion}, questionDto.GroupOfQuestion: {questionDto.GroupOfQuestion}");

                    primaryQuestionIds[key] = newQuestion.QuestionId;
                }

                // Lưu các câu hỏi không phải Primary
                foreach (var questionDto in nonPrimaryQuestions)
                {
                    var key = $"{groupOfQuestion}_{newTest.TestId}";

                    if (primaryQuestionIds.ContainsKey(key))
                    {
                        // Gán ParentQuestionId là Id của câu hỏi Primary
                        questionDto.ParentQuestionId = primaryQuestionIds[key];
                    }
                    else
                    {
                        return BadRequest($"Không tìm thấy câu hỏi Primary cho Label {questionDto.LabelOfPrimaryQuestion} trong nhóm {groupOfQuestion}");
                    }

                    var newQuestion = new Question
                    {
                        Image = questionDto.Image,
                        QuestionContent = questionDto.QuestionContent,
                        MultipleAnswer = questionDto.MultipleAnswer,
                        Primary = false,
                        ParentQuestionId = questionDto.ParentQuestionId, // Gán ParentQuestionId
                        LabelOfPrimaryQuestion = questionDto.LabelOfPrimaryQuestion,
                        IsDelete = false,
                        IsActive = true,
                        Answers = questionDto.Answers.Select(answer => new Answer
                        {
                            AnswerContent = answer.AnswerContent,
                            
                            Correct = answer.Correct,
                            Explain = answer.Explain,
                        }).ToList(),
                        PointOfQuestions = new List<PointOfQuestion>()
                    };

                    if (questionDto.PointOfQuestion.HasValue)
                    {
                        var pointOfQuestion = new PointOfQuestion
                        {
                            Point = questionDto.PointOfQuestion.Value,
                            QuestionId = newQuestion.QuestionId,
                            TestId = newTest.TestId,
                        };

                        newQuestion.PointOfQuestions.Add(pointOfQuestion);
                    }

                    _context.Questions.Add(newQuestion);
                }
            }

        _context.SaveChanges();
        return Ok(new { Message = "Sao chép bài kiểm tra thành công!", NewTestId = newTest.TestId });
    }

    [HttpPost("create")]
    public ActionResult Create(TestDto testDto)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var newTest = new Test
            {
                ApplicationUserId = testDto.UserCreate,
                TestDateCreated = DateTime.Now.ToUniversalTime(),
                TestTime = testDto.TestTime,
                PointOfTest = testDto.Point,
                IsActive = testDto.IsActive,
                IsDelete = false,
                TestName = testDto.TestName
            };


            _context.Add(newTest);

            _context.SaveChanges();
            if (testDto.StickersOfTests != null && testDto.StickersOfTests.Any())
            {
                var stickersOfTests = testDto.StickersOfTests.Select(stickerDto => new StickerOfTest
                {
                    StickerId = stickerDto.StickerId,
                    TestId = newTest.TestId,
                    IsDelete = false,
                    IsActive = true
                }).ToList();


                _context.AddRange(stickersOfTests);
            }


            if (testDto.ClassId > 0)
            {
                var testOfClass = new TestOfClass
                {
                    TestId = newTest.TestId,
                    ClassId = testDto.ClassId,
                    IsDelete = false,
                    IsActive = true
                };
                _context.Add(testOfClass);
            }


            _context.SaveChanges();


            transaction.Commit();
            var tests = _context.TestOfClasses
                .AsNoTracking()
                .Where(t => t.TestId == newTest.TestId && !t.IsDelete)
                .Include(s => s.test.StickerOfTests)
                .Select(t => new TestDto
                {
                    Id = t.test.TestId,
                    Point = t.test.PointOfTest,
                    UserCreate = t.test.applicationUser.UserName,
                    Title = t.test.TestName,
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
                }).FirstOrDefault();
            return Ok(new { Message = "Tạo bài kiểm tra thành công", NewTest = tests });
        }
        catch (Exception ex)
        {
            transaction.Rollback();


            return BadRequest(new { Message = "Có lỗi xảy ra khi tạo bài kiểm tra", Error = ex.Message });
        }
    }

    [HttpPut("update/{id}")]
    public ActionResult Update(int id, TestDto testDto)
    {
        using var transaction = _context.Database.BeginTransaction();

        try
        {
            var existingTest = _context.Tests
                .Include(t => t.StickerOfTests)
                .Include(t => t.TestOfClasses)
                .FirstOrDefault(t => t.TestId == id && !t.IsDelete);

            if (existingTest == null) return NotFound(new { Message = "Bài kiểm tra không tồn tại" });

            // Cập nhật thông tin bài kiểm tra
            existingTest.TestName = testDto.TestName;
            existingTest.TestTime = testDto.TestTime;
            existingTest.PointOfTest = testDto.Point;
            existingTest.IsActive = testDto.IsActive;

            // Cập nhật stickers
            if (testDto.StickersOfTests != null && testDto.StickersOfTests.Any())
            {
                // Xóa các StickerOfTests cũ
                foreach (var stickerOfTest in existingTest.StickerOfTests) stickerOfTest.IsDelete = true;

                // Thêm các StickerOfTests mới
                var stickersOfTests = testDto.StickersOfTests.Select(stickerDto => new StickerOfTest
                {
                    StickerId = stickerDto.StickerId,
                    TestId = existingTest.TestId,
                    IsDelete = false,
                    IsActive = true
                }).ToList();

                _context.AddRange(stickersOfTests);
            }

            // Cập nhật thông tin lớp (class) nếu có
            if (testDto.ClassId > 0)
            {
                var existingTestOfClass = _context.TestOfClasses
                    .FirstOrDefault(t =>
                        t.TestId == existingTest.TestId && t.ClassId == testDto.ClassId && !t.IsDelete);

                if (existingTestOfClass == null)
                {
                    var testOfClass = new TestOfClass
                    {
                        TestId = existingTest.TestId,
                        ClassId = testDto.ClassId,
                        IsDelete = false,
                        IsActive = true
                    };
                    _context.Add(testOfClass);
                }
            }

            _context.SaveChanges();
            transaction.Commit();

            var updatedTest = _context.TestOfClasses
                .AsNoTracking()
                .Where(t => t.TestId == existingTest.TestId && !t.IsDelete)
                .Include(s => s.test.StickerOfTests)
                .Select(t => new TestDto
                {
                    Id = t.test.TestId,
                    Point = t.test.PointOfTest,
                    UserCreate = t.test.applicationUser.UserName,
                    Title = t.test.TestName,
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
                }).FirstOrDefault();

            return Ok(new { Message = "Cập nhật bài kiểm tra thành công", UpdatedTest = updatedTest });
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return BadRequest(new { Message = "Có lỗi xảy ra khi cập nhật bài kiểm tra", Error = ex.Message });
        }
    }

    [HttpDelete]
    public IActionResult DeleteTest(int id ,int classId)
    {
        var existTest = _context.Tests.FirstOrDefault(c => c.TestId == id);
        if (existTest == null) return NotFound(new { Message = "Không tìm thấy bài kiểm tra cần xóa" });
        var existTestInClass = _context.TestOfClasses.FirstOrDefault(t => t.TestId == id 
        && t.ClassId == classId);

        if (existTestInClass == null)
        {
            return NotFound();
        }

        existTestInClass.IsDelete = true;
        existTest.IsDelete = true;
        _context.Tests.Update(existTest);
        _context.TestOfClasses.Update(existTestInClass);
        _context.SaveChanges();
        return Ok(new { Message = "Xóa thành công bài kiểm tra" });
    }
}