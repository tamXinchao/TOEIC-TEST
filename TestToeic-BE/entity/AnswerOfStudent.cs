using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class AnswerOfStudent
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Answer")]
    public int AnswerId { get; set; }
    public Answer answer { get; set; }
    
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public  Question question { get; set; }
    
    [ForeignKey("StudentPoint")]
    public int StudentPointId { get; set; } //Id bài làm
    public StudentPoint StudentPoints { get; set; }
    
    public float? PointOfAnswer { get; set; } //Điểm của câu học sinh đã làm dựa trên câu đúng và câu sai

}