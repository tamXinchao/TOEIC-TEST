using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class UserApi : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UserApi(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<ApplicationUser> Get()
    {
        return _context.Users.ToList();
    }
    
    [HttpGet("getByUsername")]
    public ActionResult<ApplicationUser> GetByUsername(string username)
    {
        var existUser = _context.Users.FirstOrDefault(u => u.UserName == username);
        if (existUser == null)
        {
            return NotFound();
        }

        return Ok(existUser);
    }

    [HttpPost]
    public ActionResult AddUserAsync(AspNetUserDto userDto)
    {
        var existUser = _context.Users.FirstOrDefault(u => u.UserName == userDto.UserName);
        if (existUser != null)
        {
            return Ok(existUser.Id);
        }
        try
        {
            // Tạo đối tượng người dùng mới
            var user = new ApplicationUser
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                NormalizedUserName = userDto.UserName.ToUpper(),
                Email = userDto.Email,
                NormalizedEmail = userDto.Email.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(), // Tự động tạo SecurityStamp
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PhoneNumber = userDto.PhoneNumber,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

            // Sử dụng PasswordHasher để mã hóa mật khẩu
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, userDto.PasswordHash);

            // Thêm người dùng vào bảng AspNetUsers
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user.Id); // Thành công
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            Console.WriteLine($"Error adding user: {ex.Message}");
            return Ok();
        }
    }
}