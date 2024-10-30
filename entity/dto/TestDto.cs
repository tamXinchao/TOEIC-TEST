namespace TestToeic.entity.dto;

public class TestDto
{
    public string UserCreate { get; set; }
    public string Title { get; set; }
    public DateTime? DateCreate { get; set; }
    public string QuestionContent { get; set; }
    public string AnswerContent { get; set; }
    public float Point { get; set; }
    public float PointOfQuestion { get; set; }
    public string? Explain { get; set; }
    public TimeOnly? TestTime { get; set; }
}