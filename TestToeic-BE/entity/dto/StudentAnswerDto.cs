namespace TestToeic.entity.dto;

public class StudentAnswerDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? StudentName { get; set; }
    public DateTime? Completion { get; set; } 
    public TimeOnly? Duration { get; set; } 
    public float? Point { get; set; } 
    public List<QuestionDto> Questions { get; set; }

    public string CompletionString
    {
        get
        {
            if (!Completion.HasValue)
            {
                return null;
            }
            var day = Completion.Value.Day;
            var month = Completion.Value.Month;
            var year = Completion.Value.Year;
            var hour = Completion.Value.Hour;
            var minutes = Completion.Value.Minute;
            string CompletionString = $"{hour} giờ {minutes} phút, Ngày {day} tháng {month} năm {year}";
            return CompletionString;
        }
    }
    
    
}