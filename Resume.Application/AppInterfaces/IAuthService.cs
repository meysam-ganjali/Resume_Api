using Resume.Application.Utility;
using Resume.SharedModel.Auth;

namespace Resume.Application.AppInterfaces;

public interface IAuthService
{
    Task<BaseResult<LoginResult>> LoginAsync(LoginParam p);
    Task<BaseResult> RegisterAsync(RegisterParam p);
}