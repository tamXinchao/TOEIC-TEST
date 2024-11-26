using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

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

    [HttpGet]
    public ActionResult<IEnumerable<StudentAnswerDto>> Get()
    {
        var studentAnswer = _context.StudentPoints.AsNoTracking()
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
                Questions = a.AnswerOfStudents.Select(q => new QuestionDto
                {
                    QuestionId = q.Question.QuestionId,
                    QuestionContent = q.Question.QuestionContent,
                    Image = q.Question.Image,
                    PointOfQuestion = q.PointOfAnswer,
                    Answers = new List<AnswerDto>
                    {
                        new AnswerDto
                        {
                            QuestionId = q.Answer.QuestionId,
                            AnswerContent = q.Answer.AnswerContent,
                            AnswerId = q.Answer.AnswerId,
                            Explain = q.Answer.Explain,
                            Correct = q.Answer.Correct
                        }
                    }

                }).ToList()
            });
        return Ok(studentAnswer);
    }
    
    [HttpPost]
    public ActionResult SaveStudentAnswer(StudentPointDto studentPointDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        float? pointOfStudent = 0;
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
        var maxTestPoint = _context.Tests.AsNoTracking()
            .FirstOrDefault(t => t.TestId == studentPointDto.TestId)?
            .PointOfTest ?? 10; 
        // Tạo danh sách tạm để chứa các AnswerOfStudentDto mới

        foreach (var questionDto in studentPointDto.AnswerOfStudentDtos)
        {
            var answer = _context.Answers.AsNoTracking()
                .FirstOrDefault(a => a.AnswerId == questionDto.AnswerId);
            var pointOfQuestion = _context.PointOfQuestions.AsNoTracking()
                .FirstOrDefault(p => p.QuestionId == questionDto.QuestionId && p.TestId == studentPointDto.TestId);

            var answerOfStudent = new AnswerOfStudent
            {
                AnswerId = questionDto.AnswerId,
                QuestionId = questionDto.QuestionId,
                StudentPointId = studentPoint.StudentPointId,
                IsActive = true,
                IsDelete = false,
            };
            if (answer == null || pointOfQuestion == null)
            {
                // Nếu không tìm thấy câu trả lời hoặc điểm câu hỏi, gán điểm là 0
                questionDto.PointOfAnswer = 0;
            }
            else
            {
                // Nếu tìm thấy câu trả lời và câu trả lời đúng, tính điểm
                if (answer.Correct == false)
                {
                    questionDto.PointOfAnswer = 0; // Nếu câu trả lời sai, gán điểm là 0
                }
                else
                {
                    // Nếu câu trả lời đúng, gán điểm bằng điểm của câu hỏi
                    questionDto.PointOfAnswer = pointOfQuestion.Point;
                }
            }
            pointOfStudent += questionDto.PointOfAnswer;
            answerOfStudent.PointOfAnswer = questionDto.PointOfAnswer;
            studentPoint.AnswerOfStudents.Add(answerOfStudent);
        }
        pointOfStudent = (float?)Math.Round(pointOfStudent.Value, 2);
        if (pointOfStudent > maxTestPoint)
        {
            pointOfStudent = maxTestPoint;
        }
        // Cập nhật danh sách AnswerOfStudentDtos trong studentPointDto sau khi vòng lặp kết thúc
        studentPoint.PointOfStudent = pointOfStudent;

        // Ghi lại thông tin vào cơ sở dữ liệu
        _context.StudentPoints.Add(studentPoint);
        _context.SaveChanges();

        return Ok(pointOfStudent);
    }
}