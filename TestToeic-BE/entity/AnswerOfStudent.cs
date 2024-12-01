using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class AnswerOfStudent : BaseEnity
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Answer")]
    public int? AnswerId { get; set; }
    public Answer Answer { get; set; }
    
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public  Question Question { get; set; }
    
    [ForeignKey("StudentPoint")]
    public int StudentPointId { get; set; }
    public StudentPoint StudentPoints { get; set; }
    
    public float? PointOfAnswer { get; set; } 
    public string? AnswerContent { get; set; }
    public string? Note { get; set; }
}