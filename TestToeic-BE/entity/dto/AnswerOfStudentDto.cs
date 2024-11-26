namespace TestToeic.entity.dto;

public class AnswerOfStudentDto
{
    public int Id { get; set; }
    public int? AnswerId { get; set; }
    public int QuestionId { get; set; }
    public int StudentPointId { get; set; }
    public float? PointOfAnswer { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}