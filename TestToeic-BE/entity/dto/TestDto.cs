using System.Globalization;
using System.Text.Json.Serialization;

namespace TestToeic.entity.dto;

public class TestDto
{
    public int Id { get; set; }
    public string UserCreate { get; set; }          
    public string? Title { get; set; }
    
    public string TestName { get; set; }
    public DateTime? DateCreate { get; set; }    
    

    public List<QuestionDto> QuestionDtos { get; set; } = new List<QuestionDto>(); // Khởi tạo mặc định là danh sách rỗng

    public float? Point { get; set; }             
    public TimeOnly? TestTime { get; set; }     

    public List<StickerDto> Stickers { get; set; } = new List<StickerDto>(); // Khởi tạo mặc định là danh sách rỗng

    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; }

    public List<TestOfClassDto> TestOfClasss { get; set; } = new List<TestOfClassDto>();

    public string? TestTimeMinutes
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

    public string? StringDateCreateTest
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
