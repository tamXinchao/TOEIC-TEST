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
    public TimeOnly? TestTime { get; set; }         
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 

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
    public string StringDateCreateTest
    {
        get
        {
            if (!DateCreate.HasValue)
            {
                return null;
            }

            var day = DateCreate.Value.Day;
            var month = DateCreate.Value.Month;
            var year = DateCreate.Value.Year;
            var hour = DateCreate.Value.Hour;
            var minutes = DateCreate.Value.Minute;
            string timeOpen = $"{hour} giờ {minutes} phút, Ngày {day} tháng {month} năm {year}";
            return timeOpen;
        }
    }
}