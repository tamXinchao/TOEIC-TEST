namespace TestToeic.entity.dto;

public class ScheduleDto
{
    public DateTime? DayOpenTest { get; set; }
    public DateTime? DayCloseTest {get;set;}
    public int  TestId { get; set; }

    public string StringDayOpenTest
    {
        get
        {
            if (!DayOpenTest.HasValue)
            {
                return null;
            }

            var day = DayOpenTest.Value.Day;
            var month = DayOpenTest.Value.Month;
            var year = DayOpenTest.Value.Year;
            var hour = DayOpenTest.Value.Hour;
            var minutes = DayOpenTest.Value.Minute;
            string timeOpen = $"{hour} giờ {minutes} phút, Ngày {day} tháng {month} năm {year}";
            return timeOpen;
        }
    }

    public string StringDayCloseTest
    {
        get
        {
            if (!DayCloseTest.HasValue)
            {
                return null;
            }

            var day = DayCloseTest.Value.Day;
            var month = DayCloseTest.Value.Month;
            var year = DayCloseTest.Value.Year;
            var hour = DayCloseTest.Value.Hour;
            var minutes = DayCloseTest.Value.Minute;
            string timeClose = $"{hour} giờ {minutes} phút, Ngày {day} tháng {month} năm {year}";
            return timeClose;
        }
    }
}