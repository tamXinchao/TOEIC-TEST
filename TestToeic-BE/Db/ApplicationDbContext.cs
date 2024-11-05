using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestToeic.entity;

namespace TestToeic.Db;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions options): base(options){}
    
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Class> Classes  { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<StudentAnswer> StudentAnswers { get; set; }
    public DbSet<Test> Tests { get; set; }




}