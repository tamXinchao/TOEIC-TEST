namespace TestToeic.entity.dto;

public class TestDto
{
    public string UserCreate { get; set; }      //TestId -> Username 
    public string StudentName { get; set; }
    public string Title { get; set; }           //Class name
    public DateTime? DateCreate { get; set; }   // Test
    public List<Question> Questions { get; set; } //Answer
    public float Point { get; set; }               //Answer
    public TimeOnly? TestTime { get; set; }     //Test
}