namespace TestToeic.entity.dto;

public class StudentPointDto
{
    public int StudentPointId { get; set; }
    public DateTime? Completion { get; set; }
    public TimeOnly? Duration { get; set; }     //Thời gian đã dùng để làm bài
    public float? PointOfStudent { get; set; }
    public int TestId { get; set; }
    public string ApplicationUserId { get; set; }
    public List<AnswerOfStudentDto> AnswerOfStudentDtos { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}