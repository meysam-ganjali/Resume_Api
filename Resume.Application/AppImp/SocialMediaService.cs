using Framework.V1.Validation;
using Microsoft.AspNetCore.Hosting;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.SocialMedia;
using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.UploadFile;
using Resume.Domain;
using Resume.SharedModel.Auth;

namespace Resume.Application.AppImp;

public class SocialMediaService : ISocialMediaService {
    private IHostingEnvironment _hostingEnvironment;
    private string _wwwRootPath;
    private string _serverPath;
    private readonly IUserSocialMediaRepository _userSocialMediaRepository;

    public SocialMediaService(IHostingEnvironment hostingEnvironment, IUserSocialMediaRepository userSocialMediaRepository) {
        _hostingEnvironment = hostingEnvironment;
        _wwwRootPath = _hostingEnvironment.WebRootPath;
        _serverPath = Path.Combine(_wwwRootPath, "assets/img/socialmedia/");
        _userSocialMediaRepository = userSocialMediaRepository;
    }
    public async Task<BaseResult> CreateAsync(CreateSocialMedia p) {
        if (await _userSocialMediaRepository.IsExistAsync(x => x.Link == p.Link && x.Name == p.Link)) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }
        string logoName = String.Empty;
        if (p.Logo.IsImage()) {
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(p.Logo.FileName);
            p.Logo.UploadToServer(imageName, _serverPath, null, null);
            logoName = imageName;
        } else {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.InvalidFileFormat
            };
        }

        var userSocialMedia = new UsersocialMedia {
            CreatedAt = DateTime.Now,
            Link = p.Link,
            Name = p.Name,
            Logo = logoName,
            UserId = p.UserId
        };
        await _userSocialMediaRepository.CreateAsync(userSocialMedia);
        await _userSocialMediaRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "شبکه مجازی با موفقیت ثبت شد."
        };
        ;
    }

    public async Task<BaseResult> EditAsync(UpdateSocialMedia p) {
        var socialMedia = await _userSocialMediaRepository.GetAsync(p.Id);
        if (socialMedia == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        }
        if (await _userSocialMediaRepository.IsExistAsync(x => (x.Link == p.Link && x.Name == p.Name) && x.Id != p.Id)) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }
        string logoName = socialMedia.Logo;
        if (p.Logo != null) {
            if (p.Logo.IsImage()) {
                if (File.Exists(_serverPath + socialMedia.Logo))
                    File.Delete(_serverPath + socialMedia.Logo);
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(p.Logo.FileName);
                p.Logo.UploadToServer(imageName, _serverPath, null, null);
                logoName = imageName;
            } else {
                return new BaseResult {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ValidationMessages.InvalidFileFormat
                };
            }
        }
        socialMedia.Link = p.Link;
        socialMedia.Name = p.Name;
        socialMedia.UpdatedAt = DateTime.Now;
        socialMedia.UserId = p.UserId;
        await _userSocialMediaRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = ValidationMessages.SuccessfulyEdit
        };
    }

    public async Task<BaseResult> DeleteAsync(Guid id, Guid userID) {
        var socialMedia = await _userSocialMediaRepository.GetUserSocial(id, userID, include: "User");
        if (socialMedia == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        }
        if (File.Exists(_serverPath + socialMedia.Logo))
            File.Delete(_serverPath + socialMedia.Logo);
        await _userSocialMediaRepository.PermanentDeleteAsync(socialMedia.Id);
        await _userSocialMediaRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "عملیات حذف پایان یافت ."
        };
    }

    public async Task<BaseResult<List<SocialMediaDto>>> GetAllAsync() {
        var socialMedias = await _userSocialMediaRepository.GetAllAsync(null, null, include: "User");
        return new BaseResult<List<SocialMediaDto>> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToUserSocialMediaDto(socialMedias)
        };
    }

    public async Task<BaseResult<List<SocialMediaDto>>> GetAllUserSocialAsync(Guid userID) {
        var socialMedias = await _userSocialMediaRepository.GetAllAsync(null, null, include: "User", filter: x => x.UserId == userID);
        if (!socialMedias.Any()) {
            return new BaseResult<List<SocialMediaDto>> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
                Message = "برای این کاربر شبکه مجازی ثبت نشده"
            };
        }

        return new BaseResult<List<SocialMediaDto>> {
            IsSuccess = true,
            Data = MappToUserSocialMediaDto(socialMedias),
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<BaseResult<SocialMediaDto>> GetAsync(Guid id) {
        var socialMedia = await _userSocialMediaRepository.GetAsync(id);
        if (socialMedia == null) {
            return new BaseResult<SocialMediaDto> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        }

        return new BaseResult<SocialMediaDto>
        {
            IsSuccess = true,
            Data = MappToSingleUserSocialMediaDto(socialMedia),
            StatusCode = HttpStatusCode.OK
        };
    }

    private SocialMediaDto MappToSingleUserSocialMediaDto(UsersocialMedia socialMedia)
    {
        return new SocialMediaDto
        {
            CreatedAt = socialMedia.CreatedAt.ToFarsi(),
            UpdatedAt = socialMedia.UpdatedAt.ToFarsi(),
            Id = socialMedia.Id,
            Link = socialMedia.Link,
            Logo = $"{_serverPath}/{socialMedia.Logo}",
            Name = socialMedia.Name,
            UserDto = MappToUserDto(socialMedia.User)
        };
    }
    private List<SocialMediaDto> MappToUserSocialMediaDto(List<UsersocialMedia> socialMedias) {
        return socialMedias.Select(x => new SocialMediaDto {
            UserDto = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            Link = x.Link,
            Logo = $"{_serverPath}/{x.Logo}",
            Name = x.Name,
            UserId = x.UserId
        }).ToList();
    }
    private UserDto MappToUserDto(User user) {
        return new UserDto {
            FullName = user.FullName,
        };
    }
}