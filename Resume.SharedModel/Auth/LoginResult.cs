using Microsoft.AspNetCore.Http;

namespace Resume.SharedModel.Auth;

public class LoginResult
{
    public string Phone { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public Guid Id { get; set; }
}

public class LoginParam
{
    public string Phone { get; set; }
    public string Password { get; set; }
}

public class RegisterParam
{
    public string Phone { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public IFormFile? Avatar { get; set; }
    public string ConfirmPassword { get; set; }
    public string Address { get; set; }
}