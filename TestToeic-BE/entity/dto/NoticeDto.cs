namespace TestToeic.entity.dto;

public class NoticeDto
{
    public int NoticeId { get; set; }
    public int ClassId { get; set; }
    public int ScoreMin { get; set; } 
    public int ScoreMax { get; set; } 
    public string Message { get; set; } = "Chưa có thông tin cụ thể";
    public int TestId { get; set; }
    public bool IsDelete { get; set; } = false;
    public bool IsActive { get; set; } = true;
}