using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace TestToeic.entity;

public class Schedule
{
    [Key]
    public int ScheduleId { get; set; }
    public DateTime? DayOpenTest { get; set; }
    public DateTime? DayCloseTest { get; set; }
    
    [ForeignKey("Test")]
    public int TestId { get; set; }
    public Test test { get; set; }
}