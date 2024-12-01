using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TestToeic.entity;

public class StudentPoint : BaseEnity
{
    [Key]
    public int StudentPointId { get; set; }
    public DateTime? Completion { get; set; }  
    public TimeOnly? Duration { get; set; }     
    public float? PointOfStudent { get; set; }
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public  ApplicationUser applicationUser { get; set; }
    public  ICollection<AnswerOfStudent> AnswerOfStudents { get; set; }
    
}