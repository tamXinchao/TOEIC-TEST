namespace TestToeic.entity.dto;

public class StickerDto
{
    public int StickerId { get; set; }
    public string StickerName { get; set; } = "Chưa có tên";
    public string? Note { get; set; }
    
    public int StickerOfTestCount { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}