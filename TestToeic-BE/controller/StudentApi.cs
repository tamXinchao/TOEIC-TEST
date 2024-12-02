using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;
using TestToeic.utils;

namespace TestToeic.controller;

[ApiController]
[Route("api/[controller]")]
public class StudentApi : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PdfExport> _logger;
    public StudentApi(ApplicationDbContext context,ILogger<PdfExport> logger)
    {
        _context = context;
        _logger = logger;

    }

    // [HttpGet]
    // public ActionResult<IEnumerable<StudentAnswerDto>> Get(string username,int? id)
    // {
    //     var existUser = _context.Users.FirstOrDefault(u => u.UserName == username);
    //     
    //
    //     if (existUser == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     IQueryable<StudentAnswerDto>? studentAnswer;
    //     if (id == null)
    //     {
    //         studentAnswer = _context.StudentPoints.AsNoTracking()
    //             .Where(u => u.ApplicationUserId == existUser.Id)
    //             .Include(s => s.AnswerOfStudents)
    //             .ThenInclude(ans => ans.Answer)
    //             .ThenInclude(q => q.question)
    //             .ThenInclude(poq => poq.PointOfQuestions)
    //             .Select(a => new StudentAnswerDto
    //             {
    //                 Id = a.StudentPointId,
    //                 PointOfStudent = a.PointOfStudent,
    //                 TestId = a.TestId,
    //                 Title = a.test.classRef.ClassName,
    //                 Completion = a.Completion,
    //                 Duration = a.Duration,
    //                 StudentName = a.applicationUser.UserName,
    //                 Questions = a.AnswerOfStudents.Select(q => new QuestionDto
    //                 {
    //                     QuestionId = q.Question.QuestionId,
    //                     QuestionContent = q.Question.QuestionContent,
    //                     Image = q.Question.Image,
    //                     PointOfQuestion = q.PointOfAnswer,
    //                     Answers = q.Answer != null
    //                         ? new List<AnswerDto>
    //                         {
    //                             new AnswerDto
    //                             {
    //                                 QuestionId = q.Answer.QuestionId, 
    //                                 AnswerContent = q.Answer.AnswerContent ,
    //                                 AnswerId = q.Answer.AnswerId,
    //                                 Explain = q.Answer.Explain ,
    //                                 Correct = q.Answer.Correct 
    //                             }
    //                         }
    //                         : new List<AnswerDto>() // Nếu `Answer` null, trả về danh sách trống
    //                 }).ToList()
    //             });
    //         return Ok(studentAnswer);
    //     }
    //     
    //     studentAnswer = _context.StudentPoints.AsNoTracking()
    //         .Where(u => u.ApplicationUserId == existUser.Id && u.StudentPointId == id)
    //         .Include(s => s.AnswerOfStudents)
    //         .ThenInclude(ans => ans.Answer)
    //         .ThenInclude(q => q.question)
    //         .ThenInclude(poq => poq.PointOfQuestions)
    //         .Select(a => new StudentAnswerDto
    //         {
    //             Id = a.StudentPointId,
    //             PointOfStudent = a.PointOfStudent,
    //             TestId = a.TestId,
    //             Title = a.test.classRef.ClassName,
    //             Completion = a.Completion,
    //             Duration = a.Duration,
    //             StudentName = a.applicationUser.UserName,
    //             Questions = a.AnswerOfStudents.Select(q => new QuestionDto
    //             {
    //                 QuestionId = q.Question.QuestionId,
    //                 QuestionContent = q.Question.QuestionContent,
    //                 Image = q.Question.Image,
    //                 PointOfQuestion = q.PointOfAnswer,
    //                 Answers = q.Answer != null
    //                     ? new List<AnswerDto>
    //                     {
    //                         new AnswerDto
    //                         {
    //                             QuestionId = q.Answer.QuestionId, 
    //                             AnswerContent = q.Answer.AnswerContent ,
    //                             AnswerId = q.Answer.AnswerId,
    //                             Explain = q.Answer.Explain ,
    //                             Correct = q.Answer.Correct 
    //                         }
    //                     }
    //                     : new List<AnswerDto>() // Nếu `Answer` null, trả về danh sách trống
    //             }).ToList()
    //         });
    //     return Ok(studentAnswer);
    // }
    
    
    [HttpGet]
    public ActionResult<IEnumerable<StudentAnswerDto>> Get(int? studentPointId)
    {
        var existUser = _context.StudentPoints.FirstOrDefault(u => u.StudentPointId == studentPointId);
        

        if (existUser == null)
        {
            return NotFound();
        }
        var studentAnswer = _context.StudentPoints.AsNoTracking()
    .Where(u => u.ApplicationUserId == existUser.ApplicationUserId && u.StudentPointId == studentPointId)
    .Include(s => s.AnswerOfStudents)
    .ThenInclude(ans => ans.Answer)
    .ThenInclude(q => q.question)
    .ThenInclude(poq => poq.PointOfQuestions)
    .Select(a => new StudentAnswerDto
    {
        Id = a.StudentPointId,
        PointOfStudent = a.PointOfStudent,
        TestId = a.TestId,
        Title = a.test.classRef.ClassName,
        Completion = a.Completion,
        Duration = a.Duration,
        StudentName = a.applicationUser.UserName,
        Questions = a.AnswerOfStudents
            .GroupBy(q => q.QuestionId) // Nhóm theo QuestionId
            .Select(group => new QuestionDto
            {
                QuestionId = group.Key, 
                QuestionContent = group.First().Question.QuestionContent, 
                Image = group.First().Question.Image, 
                PointOfQuestion = group.First().PointOfAnswer,
                Answers = group.Select(q => q.Answer != null
                    ? new AnswerDto
                    {
                        QuestionId = q.Answer.QuestionId,
                        AnswerContent = q.Answer.AnswerContent,
                        AnswerId = q.Answer.AnswerId,
                        Explain = q.Answer.Explain,
                        Correct = q.Answer.Correct
                    }
                    : new AnswerDto
                    {
                        AnswerId = -1,  // Giá trị mặc định nếu không có câu trả lời
                        AnswerContent = "Bỏ trống",  // Tùy chỉnh nội dung nếu không có câu trả lời
                        QuestionId = q.Question.QuestionId,
                        Explain = "",
                        Correct = false
                    }).ToList() // Duyệt qua từng phần tử trong nhóm
            }).ToList()
    }).FirstOrDefault();

        return Ok(studentAnswer);
    }

    [HttpGet("export")]
    public ActionResult exoprt()
    {
        string path = @"C:\DuAnCaNhan\CV\Pmedia\workspace\TestToeic\TestToeic-BE\wwwroot\resultOfStudent\demo.pdf";
        PdfWriter writer = new PdfWriter(path);
        PdfDocument pdf = new PdfDocument(writer);
        Document document = new Document(pdf);
        document.Add(new Paragraph("Super hello"));
        document.Close();
        return Ok();
    }
    
    [HttpGet("getListStudentPoint")]
    public ActionResult<IEnumerable<StudentAnswerDto>> GetByUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
        {
            var allStudentPoints = _context.StudentPoints
                .AsNoTracking()
                .Select(p => new StudentAnswerDto
                {
                    Id = p.StudentPointId,
                    Title = p.test.classRef.ClassName, // Ensure classRef is not null
                    StudentName = p.applicationUser.UserName,
                    Completion = p.Completion,
                    Duration = p.Duration,
                    PointOfStudent = p.PointOfStudent,
                })
                .ToList();

            return allStudentPoints;
        }
        var existUser = _context.Users
            .Where(u => u.UserName.ToLower().Contains(username.ToLower()) 
                        || u.Email.ToLower() == username.ToLower() 
                        || u.PhoneNumber == username)
            .ToList();

        if (existUser.Count == 0)
        {
            return NotFound("No users found matching the username.");
        }

        var poq = _context.StudentPoints
            .AsNoTracking()
            .Where(p => existUser.Select(u => u.Id).Contains(p.ApplicationUserId))
            .Select(p => new StudentAnswerDto
            {
                Id = p.StudentPointId,
                Title = p.test.classRef.ClassName, // Ensure classRef is not null
                StudentName = p.applicationUser.UserName,
                Completion = p.Completion,
                Duration = p.Duration,
                PointOfStudent = p.PointOfStudent,
            })
            .ToList();

        return poq;
    }
    
    [HttpPost]
public ActionResult SaveStudentAnswer(StudentPointDto studentPointDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    int correctAnswer = 0; 
    float? pointOfStudent = 0; 
    int totalQuestions = 0; 

    // Tạo thực thể StudentPoint
    var studentPoint = new StudentPoint
    {
        Completion = studentPointDto.Completion?.ToUniversalTime(),
        Duration = studentPointDto.Duration,
        TestId = studentPointDto.TestId,
        ApplicationUserId = studentPointDto.ApplicationUserId,
        IsDelete = false,
        IsActive = true,
        AnswerOfStudents = new List<AnswerOfStudent>()
    };
    studentPoint.applicationUser = _context.Users
        .FirstOrDefault(u => u.Id == studentPointDto.ApplicationUserId);
    totalQuestions = _context.PointOfQuestions
        .AsNoTracking()
        .Where(a => a.question.Primary == false)
        .Count(p => p.TestId == studentPointDto.TestId );
    var maxTestPoint = _context.Tests.AsNoTracking()
        .FirstOrDefault(t => t.TestId == studentPointDto.TestId)?
        .PointOfTest ?? 10; 
    foreach (var questionDto in studentPointDto.AnswerOfStudentDtos.GroupBy(q => q.QuestionId))
    {
        var questionId = questionDto.Key;
        var questionInDb = _context.Questions
            .AsNoTracking()
            .FirstOrDefault(q => q.QuestionId == questionId);

        // Nếu câu hỏi có primary = true, bỏ qua và không lưu câu trả lời
        if (questionInDb != null && questionInDb.Primary)
        {
            continue;
        }
        var answersForQuestion = questionDto.ToList(); 
        var correctAnswersInDb = _context.Answers
            .AsNoTracking()
            .Where(a => a.QuestionId == questionId && a.Correct == true)
            .ToList();

        var pointOfQuestion = _context.PointOfQuestions.AsNoTracking()
            .FirstOrDefault(p => p.QuestionId == questionId && p.TestId == studentPointDto.TestId);

        bool allAnswersCorrect = answersForQuestion.All(a =>
            correctAnswersInDb.Any(dbAnswer => dbAnswer.AnswerId == a.AnswerId));

        float point = 0;

        if (pointOfQuestion == null)
        {
            point = 0;
        }
        else if (allAnswersCorrect)
        {
            point = (float)pointOfQuestion.Point;
            correctAnswer++; 
        }

        foreach (var answerDto in answersForQuestion)
        {
            var answerOfStudent = new AnswerOfStudent
            {
                AnswerId = answerDto.AnswerId,
                QuestionId = questionId,
                StudentPointId = studentPoint.StudentPointId,
                IsActive = true,
                IsDelete = false,
                PointOfAnswer = point 
            };
            studentPoint.AnswerOfStudents.Add(answerOfStudent);
        }

        pointOfStudent += point;
    }
    pointOfStudent = (float?)Math.Round(pointOfStudent.Value, 2);
    if (pointOfStudent > maxTestPoint)
    {
        pointOfStudent = maxTestPoint;
    }

    studentPoint.PointOfStudent = pointOfStudent;

    _context.StudentPoints.Add(studentPoint);
    _context.SaveChanges();
    var notice = _context.Notices  
        .Include(n => n.classes) 
        .FirstOrDefault(n => n.TestId == studentPointDto.TestId && correctAnswer >= n.ScoreMin && correctAnswer <= n.ScoreMax);

    string message = "Chưa có thông tin cụ thể";  // Giá trị mặc định nếu không tìm thấy notice

    if (notice != null)
    {
        // Kiểm tra các đối tượng trong notice có thể là null
        message = notice.Message
            .Replace("[username]", studentPoint.applicationUser?.UserName ?? "Unknown")  // Nếu UserName là null, dùng "Unknown"
            .Replace("[class]", notice.classes?.ClassName ?? "Unknown");  // Nếu ClassName là null, dùng "Unknown"
    }
    

    // Trả về kết quả
    return Ok(new
    {
        CorrectAnswers = correctAnswer,
        TotalQuestions = totalQuestions,
        Time = studentPoint.Duration,
        Points = pointOfStudent,
        Detail = studentPoint.StudentPointId,
        ClassSuggestId= notice.ClassId,
        Message = message
    });
}

}