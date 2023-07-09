namespace Resume.SharedModel.Auth;

public class LoginResult
{
    public string Phone { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public Guid Id { get; set; }
}