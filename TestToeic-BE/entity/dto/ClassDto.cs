namespace TestToeic.entity.dto;

public class ClassDto
{
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public string StickerName { get; set; }
    public int MemberCount { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}