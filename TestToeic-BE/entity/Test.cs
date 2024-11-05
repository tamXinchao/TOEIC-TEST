using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class Test
{
    [Key]
    public int TestId { get; set; }
    public DateTime? TestDateCreated { get; set; }
    public TimeOnly? TestTime { get; set; }
    public float? Point { get; set; }
    public float? PointOfQuestion { get; set; }
    
    [ForeignKey("Class")]
    public int ClassId { get; set; }
    public Class classRef { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public ApplicationUser applicationUser { get; set; }
    
    public  ICollection<Schedule> Schedules { get; set; }
    public  ICollection<StudentAnswer> StudentAnswers { get; set; }
}