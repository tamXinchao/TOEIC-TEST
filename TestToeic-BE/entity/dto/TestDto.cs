using System.Globalization;
using System.Text.Json.Serialization;

namespace TestToeic.entity.dto;

public class TestDto
{
    public int Id { get; set; }
    public string UserCreate { get; set; }          
    public string? Title { get; set; }
    public int ClassId { get; set; }

    public string TestName { get; set; } = "Chưa có tên bài kiểm tra";
    public DateTime? DateCreate { get; set; }    
    

    public List<QuestionDto> QuestionDtos { get; set; } = new List<QuestionDto>(); // Khởi tạo mặc định là danh sách rỗng

    public float? Point { get; set; }             
    public TimeOnly? TestTime { get; set; }     

    public List<StickerDto> Stickers { get; set; } = new List<StickerDto>(); 
    public List<StickerOfTestDto> StickersOfTests { get; set; } = new List<StickerOfTestDto>(); 
    public bool? IsDelete { get; set; } 
    public bool IsActive { get; set; }

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
            if (int.TryParse(value, out int minutes))
            {
                var hour = minutes / 60;
                var minute = minutes % 60;
                TestTime = new TimeOnly(hour, minute);
            }
            else
            {
                TestTime = null;
            }
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
