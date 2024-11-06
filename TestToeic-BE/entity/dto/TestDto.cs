namespace TestToeic.entity.dto;

public class TestDto
{
    public int Id { get; set; }
    public string? UserCreate { get; set; }          //TestId -> Username 
    public string Title { get; set; }               //Class name
    public DateTime? DateCreate { get; set; }       // Test
    public List<Question> Questions { get; set; }   //Answer
    public float? Point { get; set; }                //Điểm tổng của bài test - ví dụ 10 điểm
    public TimeOnly? TestTime { get; set; }         // Thời gian làm bài
}