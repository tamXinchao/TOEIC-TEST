namespace TestToeic.entity.dto;

public class AnswerDto
{
    public int AnswerId { get; set; }
    public int QuestionId { get; set; }
    public Boolean Correct { get; set; }
    public string AnswerContent { get; set; }
    public string? Explain { get; set; }
}