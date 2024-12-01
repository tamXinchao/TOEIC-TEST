using System.ComponentModel.DataAnnotations;

namespace TestToeic.entity;

public class Sticker : BaseEnity
{
    [Key]
    public int StickerId { get; set; }
    public string StickerName { get; set; }
    public string? Note { get; set; }
    
    public ICollection<StickerOfTest> StickerOfTests { get; set; }
}