using System.ComponentModel.DataAnnotations;

namespace TestToeic.entity;

public class Class
{
    [Key]
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    
    public  ICollection<Test> Tests { get; set; }
}