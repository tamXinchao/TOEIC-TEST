using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestToeic.entity;

public class StudentAnswer
{
    [Key]
    public int StudentAnswerId { get; set; }
    public float StudentAnswerPoint { get; set; }
    public DateTime? Completion { get; set; } 
    public TimeOnly? Duration { get; set; } 
    
    
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; }
    
    [ForeignKey("Answer")]
    public int AnswerId { get; set; }
    public Answer answer { get; set; }
    
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public  Question question { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public int UserId { get; set; }
    public  ApplicationUser applicationUser { get; set; }
    
    
}