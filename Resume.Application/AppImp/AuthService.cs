using Microsoft.IdentityModel.Tokens;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Application.Utility.JWTSecret;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Framework.V1.HashTool;
using Framework.V1.UploadFile;
using Framework.V1.Validation;
using Microsoft.Extensions.Options;
using Resume.Domain;

namespace Resume.Application.AppImp;

public class AuthService : IAuthService {
    private readonly IAuthRepository _authRepository;
    private readonly AppSetting _appSettings;
    public AuthService(IAuthRepository authRepository, IOptions<AppSetting> appSetting) {
        _authRepository = authRepository;
        _appSettings = appSetting.Value;
    }
    public async Task<BaseResult<LoginResult>> LoginAsync(LoginParam p) {
        var user = await _authRepository.getUser(x => x.Phone == p.Phone, "Role");
        // return null if user not found
        if (user == null) {
            return new BaseResult<LoginResult>() {
                IsSuccess = false,
                Data = null,
                Message = ValidationMessages.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        PasswordHasher passwordHasher = new PasswordHasher();
        var verifyPass = passwordHasher.VerifyPassword(user.Password, p.Password);
        if (!verifyPass) {
            return new BaseResult<LoginResult> {
                IsSuccess = false,
                Data = null,
                Message = ValidationMessages.WrongUserPass,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        // authentication successful so generate jwt token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var claims = new ClaimsIdentity();
        claims.AddClaims(new[]
        {
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
        });

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = claims,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        user.Token = tokenHandler.WriteToken(token);
        await _authRepository.SaveAsync();
        // remove password before returning
        user.Password = null;

        return new BaseResult<LoginResult>() {
            IsSuccess = true,
            Message = "Login Is Successfuly...",
            StatusCode = HttpStatusCode.OK,
            Data = new LoginResult {
                Phone = user.Phone,
                Role = user.Role.Name,
                Id = user.Id,
                Token = tokenHandler.WriteToken(token)
            }
        };
    }

    public async Task<BaseResult> RegisterAsync(RegisterParam p)  {
        if (await _authRepository.IsExistAsync(x => x.Phone == p.Phone)) {
            return new BaseResult {
                IsSuccess = false,
                Message = "User Is Exist...",
                StatusCode = HttpStatusCode.NotFound
            };
        }
        if (p.Password != p.ConfirmPassword) {
            return new BaseResult {
                IsSuccess = false,
                Message = "کلمه عبور با تکرار آن برابر نیست",
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        if (p.Password.Length < 5) {
            return new BaseResult {
                IsSuccess = false,
                Message = "Password is less than 5 characters...",
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        string picName = string.Empty;
        if (p.Avatar != null) {
            if (p.Avatar.IsImage()) {
                var serverPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/assets/img/users/avatar/");
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(p.Avatar.FileName);
                p.Avatar.UploadToServer(imageName, serverPath, null, null);
                picName = imageName;
            } else {
                return new BaseResult() {
                    IsSuccess = false,
                    Message = "picture Not Valid",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
        var passwordHasher = new PasswordHasher();
        string hasPassword = passwordHasher.HashPassword(p.ConfirmPassword);
        var user = new User() {
            CreatedAt = DateTime.Now,
            Avatar = picName,
            Phone = p.Phone,
            FullName = p.FullName,
            Password = hasPassword,
            RoleID = Guid.Parse("444B1AA7-9477-4D72-B547-E3A062FD8ADD"),
            Status = true,
            Address = p.Address
        };
        await _authRepository.CreateAsync(user);
        await _authRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            Message = "Register Is Successfuly...",
            StatusCode = HttpStatusCode.OK
        };
    }
}