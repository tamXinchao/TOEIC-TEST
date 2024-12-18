using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class Class : BaseEnity
{
    [Key]
    public int ClassId { get; set; }

    public string ClassName { get; set; } = "Chưa có tên lớp";
    
    [ForeignKey("LevelOfClass")]
    public int? LevelOfClassId { get; set; }
    public LevelOfClass LevelOfClasses { get; set; } = new LevelOfClass();
    
    public  ICollection<MemberOfClass> StudentOfClasses { get; set; }
    public  ICollection<TestOfClass> TestOfClasses { get; set; }
    public  ICollection<Notice> Notices { get; set; }
}