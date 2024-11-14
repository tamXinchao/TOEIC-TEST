namespace TestToeic.entity;

public abstract class BaseEnity
{
    public bool IsDelete { get; set; } = false; // Hoặc true nếu cần
    public bool IsActive { get; set; } = true;  // Ví dụ, mặc định là "kích hoạt"

}