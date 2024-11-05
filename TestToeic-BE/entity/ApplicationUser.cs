using Microsoft.AspNetCore.Identity;

namespace TestToeic.entity;

public class ApplicationUser : IdentityUser
{
    
    public  ICollection<StudentAnswer> StudentAnswers { get; set; }
    public  ICollection<Test> Tests { get; set; }


}