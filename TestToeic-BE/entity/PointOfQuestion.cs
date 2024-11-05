using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class PointOfQuestion
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; }
    
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public Question question { get; set; }
    
    public float? Point { get; set; }
}