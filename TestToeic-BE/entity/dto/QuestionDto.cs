namespace TestToeic.entity.dto;

public class QuestionDto
{
    public int QuestionId { get; set; }
    public string? QuestionContent { get; set; }
    public float? PointOfQuestion { get; set; }
    public bool MultipleAnswer { get; set; }
    public bool Primary { get; set; }
    public int? ParentQuestionId { get; set; }
    public int GroupOfQuestion { get; set; }
    public string LabelOfPrimayQuestion { get; set; } = "Phần";
    public string? Image { get; set; }
    public List<AnswerDto>? Answers { get; set; }
}