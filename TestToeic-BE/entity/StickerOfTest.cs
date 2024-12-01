using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class StickerOfTest : BaseEnity
{
    [Key]
    public int StickerOfTestId { get; set; }
    
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; }
    
    [ForeignKey("Sticker")]
    public int StickerId { get; set; }
    public Sticker sticker { get; set; }
}