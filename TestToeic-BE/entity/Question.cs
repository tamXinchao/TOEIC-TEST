using System.ComponentModel.DataAnnotations;

namespace TestToeic.entity;

public class Question : BaseEnity
{
    [Key]
    public int QuestionId { get; set; }
    public string QuestionContent { get; set; }
    public string? Image { get; set; }
    public  ICollection<Answer> Answers { get; set; }
    public  ICollection<PointOfQuestion> PointOfQuestions { get; set; }
    public  ICollection<AnswerOfStudent> AnswerOfStudents { get; set; }
}
