using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class Notice : BaseEnity
{
    [Key]
    public int NoticeId { get; set; }

    public int ScoreMin { get; set; } = 0;
    public int ScoreMax { get; set; } = 0;
    
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; } =  new Test();

    public string Message { get; set; } = "Chưa có thông tin cụ thể";
    
    [ForeignKey("Class")]
    public int ClassId { get; set; }

    public Class classes { get; set; } = new Class();

}