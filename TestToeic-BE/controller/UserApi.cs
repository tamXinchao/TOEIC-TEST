using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestToeic.Db;
using TestToeic.entity;
using TestToeic.entity.dto;

namespace TestToeic.controller;

[Route("api/[controller]")]
[ApiController]
public class UserApi : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserApi(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public List<ApplicationUser> Get()
    {
        return _context.Users.ToList();
    }
    
    [HttpPost("login")]
    public ActionResult GetByUsername(UserDto loginUser)
    {
        var existUser = _context.Users.FirstOrDefault(u => u.UserName == loginUser.Username);
        if (existUser == null)
        {
            return NotFound("Tài khoản hoặc mật khẩu không đúng vui lòng thử lại");
        }

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var checkPass = passwordHasher.VerifyHashedPassword(existUser, existUser.PasswordHash, loginUser.Password);
        if (checkPass == PasswordVerificationResult.Success)
        {
            var dto = new UserDto
            {
                UserId = existUser.Id,
                Email = existUser.Email,
                Username = existUser.UserName ?? "Chưa có tên người dùng"
            };
            var userRoles = _context.UserRoles
                .Where(ur => ur.UserId == existUser.Id)
                .Select(ur => ur.RoleId)
                .ToList();
            var role = _context.Roles.Where(r => userRoles.Contains(r.Id))
                .Select(r => r.Name)
                .ToList();

            bool hasTeacherRole = role.Contains("teacher");
            dto.Permission = hasTeacherRole;
            dto.RoleName = hasTeacherRole ? "teacher" : string.Join(", ", role);
            var token = GenerateJwtToken(dto);
            return Ok(token);
        }
        else
        {
            return Unauthorized("Tên đăng nhập hoặc mật khẩu không chính xác");
        }
        
    }
    public string GenerateJwtToken(UserDto dto)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSetting:Subject"]!),
            new Claim("UserId" ,dto.UserId),
            new Claim("Username", dto.Username),
            new Claim("Permission", dto.Permission ? "Yes" : "No"),
            new Claim("Role", dto.RoleName)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSetting:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7).ToUniversalTime(),
            Issuer = _configuration["JwtSetting:Issuer"],
            Audience=  _configuration["JwtSetting:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine(token);
        return tokenHandler.WriteToken(token);
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
            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                EmailConfirmed = true,
                PhoneNumber = userDto.PhoneNumber,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                NormalizedUserName = userDto.UserName?.ToUpper(),
                NormalizedEmail = userDto.Email?.ToUpper()
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, userDto.PasswordHash);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding user: {ex.Message}");
            return Ok();
        }
    }
}