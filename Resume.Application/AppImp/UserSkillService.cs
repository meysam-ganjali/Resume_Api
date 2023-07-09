using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.UploadFile;
using Framework.V1.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Auth;
using Resume.SharedModel.Skills;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.Application.AppImp;

public class UserSkillService : IUserSkillService {
    private readonly IUserSkillRepository _userSkillRepository;
    private IHostingEnvironment _hostingEnvironment;
    private string _wwwRootPath;
    private string _serverPath;
    public UserSkillService(IUserSkillRepository userSkillRepository, IHostingEnvironment hostingEnvironment) {
        _userSkillRepository = userSkillRepository;
        _hostingEnvironment = hostingEnvironment;
        _wwwRootPath = _hostingEnvironment.WebRootPath;
        _serverPath = Path.Combine(_wwwRootPath, "assets/img/skills/");
    }
    public async Task<BaseResult<List<SkillDto>>> GetAllAsync() {
        var skills = await _userSkillRepository.GetAllAsync(null, null, include: "SkillType,User");
        return new BaseResult<List<SkillDto>> {
            Data = MappToSkillsDto(skills),
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }


    public async Task<BaseResult<List<SkillDto>>> GetAllUserSkillAsync(Guid userId) {
        var skills = await _userSkillRepository.GetUserSkills(userId, include: "SkillType,User");
        if (!skills.Any()) {
            return new BaseResult<List<SkillDto>> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
                Message = $"مهارتی  یافت نشد"
            };
        }

        return new BaseResult<List<SkillDto>> {
            StatusCode = HttpStatusCode.OK,
            Data = MappToSkillsDto(skills),
            IsSuccess = true
        };
    }

    public async Task<BaseResult<SkillDto>> GetAsync(Guid id) {
        Skill? skill = await _userSkillRepository.GetAsync(id, include: "SkillType,User");
        if (skill == null) {
            return new BaseResult<SkillDto> {
                IsSuccess = false,
                Data = null,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
            };
        }

        return new BaseResult<SkillDto> {
            Data = MappToSkillDto(skill),
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true
        };
    }

    public async Task<BaseResult> CreateAsync(CreateSkillParam p) {
        if (p.SkillLogo == null) {
            return new BaseResult() {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = "لوگوی مهارت یافت نشد",
            };
        }

        if (await _userSkillRepository.IsExistAsync(x => x.SkillName == p.SkillName && x.UserId == p.UserId)) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }

        string logoName = String.Empty;
        if (p.SkillLogo.IsImage()) {
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(p.SkillLogo.FileName);
            p.SkillLogo.UploadToServer(imageName, _serverPath, null, null);
            logoName = imageName;
        } else {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.InvalidFileFormat
            };
        }

        Skill skill = new Skill {
            CreatedAt = DateTime.Now,
            SkillName = p.SkillName,
            SkillLogo = logoName,
            SkillPercentage = p.SkillPercentage,
            SkillTypeId = p.SkillTypeId,
            UserId = p.UserId,

        };
        await _userSkillRepository.CreateAsync(skill);
        await _userSkillRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "عملیات ثبت مهارت با موفقیت به اتمام رسید."
        };
    }

    public async Task<BaseResult> EditAsync(UpdateSkillParam p) {
        Skill? skill = await _userSkillRepository.GetAsync(p.Id);
        if (skill == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
            };
        }
        if (await _userSkillRepository.IsExistAsync(x => (x.SkillName == p.SkillName && x.UserId == p.UserId) && x.Id != p.Id)) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }

        string logoName = skill.SkillLogo;
        if (p.SkillLogo != null) {
            if (p.SkillLogo.IsImage()) {
                if (File.Exists(_serverPath + skill.SkillLogo))
                    File.Delete(_serverPath + skill.SkillLogo);
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(p.SkillLogo.FileName);
                p.SkillLogo.UploadToServer(imageName, _serverPath, null, null);
                logoName = imageName;
            } else {
                return new BaseResult {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = ValidationMessages.InvalidFileFormat
                };
            }
        }
        skill.SkillName = p.SkillName;
        skill.SkillLogo = logoName;
        skill.SkillPercentage = p.SkillPercentage;
        skill.UserId = p.UserId;
        skill.SkillTypeId = p.SkillTypeId;
        await _userSkillRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = ValidationMessages.SuccessfulyEdit
        };
    }

    public async Task<BaseResult> DeleteAsync(Guid id,Guid userId)
    {
        Skill? skill = await _userSkillRepository.GetUserSkill(id, userId);
        if (skill == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
            };
        }
        if (File.Exists(_serverPath + skill.SkillLogo))
            File.Delete(_serverPath + skill.SkillLogo);
        await _userSkillRepository.PermanentDeleteAsync(skill.Id);
        await _userSkillRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "عملیات حذف به پایان رسید",
        };
    }


    private List<SkillDto> MappToSkillsDto(List<Skill> skills) {


        return skills.Select(x => new SkillDto() {
            User = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            SkillType = MappToSkillType(x.SkillType),
            SkillLogo = $"{_serverPath}/{x.SkillLogo}",
            SkillName = x.SkillName,
            SkillPercentage = x.SkillPercentage,
        }).ToList();
    }
    private SkillTypeDto MappToSkillType(SkillType skillType) {
        return new SkillTypeDto {
            CreatedAt = skillType.CreatedAt.ToFarsi(),
            UpdatedAt = skillType.UpdatedAt.ToFarsi(),
            Id = skillType.Id,
            Name = skillType.Name,
        };
    }
    private UserDto MappToUserDto(User user) {
        return new UserDto {
            Id = user.Id,
            FullName = user.FullName
        };
    }
    private SkillDto MappToSkillDto(Skill skill) {
        return new SkillDto {
            User = MappToUserDto(skill.User),
            CreatedAt = skill.CreatedAt.ToFarsi(),
            UpdatedAt = skill.UpdatedAt.ToFarsi(),
            Id = skill.Id,
            SkillType = MappToSkillType(skill.SkillType),
            SkillLogo = $"{_serverPath}/{skill.SkillLogo}",
            SkillName = skill.SkillName,
            SkillPercentage = skill.SkillPercentage,

        };
    }

}