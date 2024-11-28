namespace TestToeic.entity.dto;

public class StudentOfClassDto
{
    public int MemberOfClassId { get; set; }
    public string ApplicationUserId { get; set; }
    public int ClassId { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}