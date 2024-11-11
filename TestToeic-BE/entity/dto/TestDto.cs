using System.Globalization;

namespace TestToeic.entity.dto;

public class TestDto
{
    public int Id { get; set; }
    public string? UserCreate { get; set; }          //TestId -> Username 
    public string Title { get; set; }               //Class name
    public DateTime? DateCreate { get; set; }       // Test
    public List<QuestionDto> QuestionDtos { get; set; }   //Answer
    public float? Point { get; set; }                //Điểm tổng của bài test - ví dụ 10 điểm
    public TimeOnly? TestTime { get; set; }         // Thời gian làm bài

    public string TestTimeMinutes
    {
        get
        {
            if (!TestTime.HasValue)
            {
                return null;
            }

            var hour = TestTime.Value.Hour;
            var minute = TestTime.Value.Minute;
            var TimeMinutes = (hour * 60) + minute;

            return TimeMinutes.ToString();
        }
        set
        {
            TestTime = value != null ? TimeOnly.ParseExact(value, "t", CultureInfo.CurrentCulture) : null;
        }
    }
}