using Microsoft.AspNetCore.Identity;

namespace TestToeic.entity;

public class ApplicationUser : IdentityUser
{
    public  ICollection<StudentPoint> StudentPoints { get; set; }
    public  ICollection<Test> Tests { get; set; }


}