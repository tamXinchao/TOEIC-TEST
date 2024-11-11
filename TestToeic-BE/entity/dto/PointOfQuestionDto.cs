namespace TestToeic.entity.dto;

public class PointOfQuestionDto
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public int QuestionId { get; set; }
    public float? Point { get; set; }
}