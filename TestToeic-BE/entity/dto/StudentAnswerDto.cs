namespace TestToeic.entity.dto;

public class StudentAnswerDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string StudentName { get; set; }
    public DateTime? Completion { get; set; } //Ngày, giờ hoàn thành bài kiểm tra
    public TimeOnly? Duration { get; set; } //Hoàn thành trong khoảng thời gian
    public float Point { get; set; } //Điểm của thí sinh làm bài
    public int AnswerId { get; set; }
    public List<Question> Questions { get; set; }
    public List<Answer> Answers { get; set; }
    
    
}