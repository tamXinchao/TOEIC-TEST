using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class TestOfClass : BaseEnity
{
    [Key]
    public int TestOfClassId { get; set; }
    
    [ForeignKey("Test")]
    public int TestId { get; set; }

    public Test test { get; set; } 
    
    [ForeignKey("Class")]
    public int ClassId { get; set; }
    public Class classRef { get; set; } 
}