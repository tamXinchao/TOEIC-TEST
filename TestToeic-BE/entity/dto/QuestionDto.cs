namespace TestToeic.entity.dto;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string QuestionContent { get; set; }
    public float? PointOfQuestion { get; set; }
    public string? Image { get; set; }
    public List<AnswerDto>? Answers { get; set; }
}