using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestToeic.entity;

public class StudentPoint
{
    [Key]
    public int StudentAnswerId { get; set; }
    public float StudentAnswerPoint { get; set; }
    public DateTime? Completion { get; set; } 
    public TimeOnly? Duration { get; set; } 
    public float? PointOfStudent { get; set; }
    
    public  ICollection<AnswerOfStudent> AnswerOfStudents { get; set; }
    
}