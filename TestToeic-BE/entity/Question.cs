using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestToeic.entity;

public class Question : BaseEnity
{
    [Key]
    public int QuestionId { get; set; }
    public string QuestionContent { get; set; }
    public string? Image { get; set; }
    public bool MultipleAnswer { get; set; }
    public bool Primary { get; set; }
    public string LabelOfPrimary { get; set; } = "Phần";
    public int? ParentQuestionId { get; set; }
    [ForeignKey("ParentQuestionId")]
    public Question ParentQuestion { get; set; }
    
    public  ICollection<Answer> Answers { get; set; }
    public  ICollection<PointOfQuestion> PointOfQuestions { get; set; }
    public  ICollection<AnswerOfStudent> AnswerOfStudents { get; set; }
    public ICollection<Question> SubQuestions { get; set; }
}
