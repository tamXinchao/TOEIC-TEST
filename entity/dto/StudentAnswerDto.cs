namespace TestToeic.entity.dto;

public class StudentAnswerDto
{
    public string Title { get; set; }
    public string StudentName { get; set; }
    public DateTime? Completion { get; set; }
    public TimeOnly? Duration { get; set; }
    public float Point { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    
}