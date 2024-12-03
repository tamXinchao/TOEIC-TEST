namespace TestToeic.entity.dto;

public class MemberOfClassDto
{
    public int MemberOfClassId { get; set; }
    public string MemberName { get; set; } = "Thành viên";
    public string MemberId { get; set; } = "";
    public int ClassId { get; set; } 
    
    public string ClassName { get; set; } = "Lớp";
    public DateTime JoinDate { get; set; } = DateTime.Now;

    public bool IsDelete { get; set; } = false;
    public bool IsActive { get; set; } = true;
    
    
    public string GetFormattedJoinDate
    {
        get
        {
        // Lấy ngày, tháng và năm
        string day = JoinDate.Day.ToString("D2");  // Format 2 chữ số cho ngày
        string month = JoinDate.ToString("MMM", new System.Globalization.CultureInfo("vi-VN"));  // Tháng viết tắt với chữ hoa
        string year = JoinDate.Year.ToString();
            
        // Lấy thứ trong tuần
        string dayOfWeek = JoinDate.ToString("dddd", new System.Globalization.CultureInfo("vi-VN"));
        dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1); 

        // Trả về chuỗi theo định dạng yêu cầu
        return $"{day} {month} {year} {dayOfWeek}";
        }
    }
}