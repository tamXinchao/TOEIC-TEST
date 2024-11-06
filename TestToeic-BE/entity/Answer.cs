using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class Answer
{
    [Key]
    public int AnswerId { get; set; }
    public Boolean Correct { get; set; }
    public string AnswerContent { get; set; }
    public string? Explain { get; set; }
    
    [ForeignKey("Question")]
    public int QuestionId { get; set; }
    public  Question question { get; set; }

    public  ICollection<AnswerOfStudent> AnswerOfStudents { get; set; }
}