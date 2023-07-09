using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.Validation;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Auth;
using Resume.SharedModel.WorkExperience;

namespace Resume.Application.AppImp;

public class WorkExperienceService : IWorkExperienceService {
    private readonly IWorkExperienceRepository _workExperienceRepository;

    public WorkExperienceService(IWorkExperienceRepository workExperienceRepository) {
        _workExperienceRepository = workExperienceRepository;
    }
    public async Task<BaseResult> CreateAsync(CreateWorkExperience p) {
        if (await _workExperienceRepository.IsExistAsync(x => x.UserId == p.UserId && x.CompanyName == p.CompanyName)) {
            return new BaseResult() {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }

        var workExperience = new WorkExperience {
            CreatedAt = DateTime.Now,
            CompanyName = p.CompanyName,
            Experience = p.Experience,
            JobPossition = p.JobPossition,
            UserId = p.UserId,
        };
        await _workExperienceRepository.CreateAsync(workExperience);
        await _workExperienceRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "سابقه جدید به لیست سوابق با موفقیت اضافه شد"
        };
    }

    public async Task<BaseResult> EditAsync(UpdateWorkExperience p) {
        var workExperience = await _workExperienceRepository.GetAsync(p.Id);
        if (workExperience == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        }
        if (await _workExperienceRepository.IsExistAsync(x => (x.UserId == p.UserId && x.CompanyName == p.CompanyName) && x.Id != p.Id)) {
            return new BaseResult() {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        }
        workExperience.UserId = p.UserId;
        workExperience.CompanyName = p.CompanyName;
        workExperience.Experience = p.Experience;
        workExperience.JobPossition = p.JobPossition;
        workExperience.UpdatedAt = DateTime.Now;
        await _workExperienceRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = ValidationMessages.SuccessfulyEdit
        };
    }

    public async Task<BaseResult<List<WorkExperienceDto>>> GetAllAsync() {
        var workExperiences = await _workExperienceRepository.GetAllAsync(null, null,include: "User");
        return new BaseResult<List<WorkExperienceDto>> {
            Data = MappToWorkExperiencesDto(workExperiences),
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true
        };
    }

    public async Task<BaseResult<List<WorkExperienceDto>>> GetAllUserWorkExperienceAsync(Guid userID) {
        var workExperienceDtos = await _workExperienceRepository.GetAllAsync(null, null, filter: x => x.UserId == userID, include: "User");
        if (!workExperienceDtos.Any())
            return new BaseResult<List<WorkExperienceDto>> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
                Message = "سابقه شغلی برای کاربر یافت نشد"
            };
        return new BaseResult<List<WorkExperienceDto>> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToWorkExperiencesDto(workExperienceDtos),
        };
    }

    public async Task<BaseResult<WorkExperienceDto>> GetAsync(Guid id) {
        var workExperience = await _workExperienceRepository.GetAsync(id, include: "User");
        if (workExperience == null) {
            return new BaseResult<WorkExperienceDto> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
                Data = null
            };
        }

        return new BaseResult<WorkExperienceDto> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToWorkExperienceDto(workExperience)
        };
    }

    public async Task<BaseResult> DeleteAsync(Guid id, Guid userId) {
        var workExperience = await _workExperienceRepository.GetUserWorkExperience(id, userId);
        if (workExperience == null) {
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
            };
        }

        await _workExperienceRepository.PermanentDeleteAsync(workExperience.Id);
        await _workExperienceRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "عملیات حذف به پایان رسید",
        };
    }

    private List<WorkExperienceDto> MappToWorkExperiencesDto(List<WorkExperience> workExperiences) {
        return workExperiences.Select(x => new WorkExperienceDto {
            User = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            UserId = x.UserId,
            CompanyName = x.CompanyName,
            Experience = x.Experience,
            JobPossition = x.JobPossition,
        }).ToList();
    }
    private WorkExperienceDto MappToWorkExperienceDto(WorkExperience x) {
        return new WorkExperienceDto {
            User = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            UserId = x.UserId,
            CompanyName = x.CompanyName,
            Experience = x.Experience,
            JobPossition = x.JobPossition,
        };
    }

    private UserDto MappToUserDto(User user) {
        return new UserDto {
            FullName = user.FullName,
        };
    }
}