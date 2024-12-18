using System.Runtime.CompilerServices;

namespace TestToeic.entity.dto;

public class AspNetUserDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            NormalizedUserName = value.ToUpper();
        }
    }

    public string NormalizedUserName { get; private set; }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            NormalizedEmail = value.ToUpper();
        }
    }

    public string NormalizedEmail { get; private set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int AccessFailedCount { get; set; }
}
