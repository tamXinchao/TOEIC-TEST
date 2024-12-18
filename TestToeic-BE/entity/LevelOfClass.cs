using System.ComponentModel.DataAnnotations;

namespace TestToeic.entity;

public class LevelOfClass : BaseEnity
{
    [Key]
    public int LevelOfClassId { get; set; }

    public string LevelName { get; set; } = "Chưa có tên người cấp độ";
    
    public ICollection<Class> Classes { get; set; }
}