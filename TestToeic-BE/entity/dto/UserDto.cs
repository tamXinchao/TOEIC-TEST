namespace TestToeic.entity.dto;

public class UserDto : BaseEnity
{
    public string? UserId { get; set; }
    public string Username { get; set; } = "Chưa có tên người dùng";
    public string PhoneNumber { get; set; } = "Chưa có tên người dùng";
    public string? Email { get; set; } = "Chưa có Email người dùng";
    public bool Permission { get; set; }
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; } = "Người dùng chưa có vai trò";
    public string Password { get; set; } = "123";
}