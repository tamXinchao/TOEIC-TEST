using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class MemberOfClass : BaseEnity
{
    [Key]
    public int MemberOfClassId { get; set; }
    
    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; }
    public ApplicationUser applicationUser { get; set; }
    
    [ForeignKey("Class")]
    public int ClassId { get; set; }
    public Class classRef { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.Now;
}