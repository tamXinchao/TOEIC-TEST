namespace TestToeic.entity.dto;

public class TestOfClassDto
{
    public int TestOfClassId { get; set; }
    public int TestId { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public bool? IsDelete { get; set; } 
    public bool? IsActive { get; set; } 
}