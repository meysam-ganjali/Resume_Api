using Microsoft.AspNetCore.Http;

namespace Resume.SharedModel.Auth;

public class RegisterParam
{
    public string Phone { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public IFormFile? Avatar { get; set; }
    public string ConfirmPassword { get; set; }
    public string Address { get; set; }
}