namespace TestToeic.entity.dto;

public class StickerOfTestDto
{
    public int StickerOfTestId { get; set; }
    public int TestId { get; set; }
    public int StickerId { get; set; }
    public bool IsDelete { get; set; } = false; 
    public bool IsActive { get; set; } = true;  
}