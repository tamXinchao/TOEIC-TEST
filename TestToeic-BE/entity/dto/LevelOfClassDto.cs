namespace TestToeic.entity.dto;

public class LevelOfClassDto : BaseEnity
{
    public int LevelOfClassId { get; set; }
    public string LeveName { get; set; } = "Chưa có tên cho cấp độ";
    public int ClassCount { get; set; }
}