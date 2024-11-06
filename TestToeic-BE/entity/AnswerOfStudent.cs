using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class AnswerOfStudent
{
    [Key]
    public int Id { get; set; }
    
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
    public string ApplicationUserId { get; set; }
    public  ApplicationUser applicationUser { get; set; }
    
    public float? PointOfAnswer { get; set; }

}