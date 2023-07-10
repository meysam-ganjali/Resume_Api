using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.Validation;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Auth;
using Resume.SharedModel.DegreeEducations;

namespace Resume.Application.AppImp;

public class DegreeEducationService : IDegreeEducationService {
    private readonly IDegreeEducationRepository _degreeEducationRepository;

    public DegreeEducationService(IDegreeEducationRepository degreeEducationRepository) {
        _degreeEducationRepository = degreeEducationRepository;
    }
    public async Task<BaseResult> CreateAsync(CreateDegreeEducation p) {
        if (await _degreeEducationRepository.IsExistAsync(x =>
                x.UserId == p.UserId && x.GraduationYear == p.GraduationYear))
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        var degreeEducation = new DegreeEducation {
            CreatedAt = DateTime.Now,
            FieldStudy = p.FieldStudy,
            GraduationYear = p.GraduationYear,
            TotalAverage = p.TotalAverage,
            UserId = p.UserId,
            UniversityName = p.UniversityName
        };
        await _degreeEducationRepository.CreateAsync(degreeEducation);
        await _degreeEducationRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "سابقه تحصیلی با موفقیت ثبت شد"
        };
    }

    public async Task<BaseResult> UpdateAsync(UpdateDegreeEducation p) {
        var degreeEducation = await _degreeEducationRepository.GetAsync(p.Id, include: "User");
        if (degreeEducation == null)
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        if (await _degreeEducationRepository.IsExistAsync(x =>
               (x.UserId == p.UserId && x.GraduationYear == p.GraduationYear) && x.Id != p.Id))
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Message = ValidationMessages.DuplicatedRecord
            };
        degreeEducation.UserId = p.UserId;
        degreeEducation.GraduationYear = p.GraduationYear;
        degreeEducation.FieldStudy = p.FieldStudy;
        degreeEducation.TotalAverage = p.TotalAverage;
        degreeEducation.UpdatedAt = DateTime.Now;
        degreeEducation.UniversityName = p.UniversityName;
        await _degreeEducationRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = ValidationMessages.SuccessfulyEdit
        };
    }

    public async Task<BaseResult> DeleteAsync(Guid id, Guid userId) {
        var degreeEducation = await _degreeEducationRepository.GetUserDegreeEducation(id, userId, include: "User");
        if (degreeEducation == null)
            return new BaseResult {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound
            };
        await _degreeEducationRepository.PermanentDeleteAsync(degreeEducation.Id);
        await _degreeEducationRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "عملیات حذف با موفقیت به اتمام رسید"
        };
    }

    public async Task<BaseResult<List<DegreeEducationDto>>> GetAllAsync() {
        var degreeEducations = await _degreeEducationRepository.GetAllAsync(null, null, include: "User");
        return new BaseResult<List<DegreeEducationDto>> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToDegreeEducationList(degreeEducations)
        };
    }

    public async Task<BaseResult<DegreeEducationDto>> GetAsync(Guid id) {
        var degreeEducation = await _degreeEducationRepository.GetAsync(id, include: "User");
        if (degreeEducation == null)
            return new BaseResult<DegreeEducationDto> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
                Data = null
            };
        return new BaseResult<DegreeEducationDto> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToDegreeEducation(degreeEducation)
        };
    }

    public async Task<BaseResult<List<DegreeEducationDto>>> GetAllUserDegreeEducationAsync(Guid userId) {
        var degreeEducations = await _degreeEducationRepository.GetAllAsync(null, null, include: "User", filter: x => x.UserId == userId);
        if (!degreeEducations.Any())
            return new BaseResult<List<DegreeEducationDto>> {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                Message = ValidationMessages.RecordNotFound,
                Data = null
            };
        return new BaseResult<List<DegreeEducationDto>> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Data = MappToDegreeEducationList(degreeEducations)
        };
    }

    private List<DegreeEducationDto> MappToDegreeEducationList(List<DegreeEducation> degreeEducations) {
        return degreeEducations.Select(x => new DegreeEducationDto {
            User = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            UserId = x.UserId,
            GraduationYear = x.GraduationYear,
            FieldStudy = x.FieldStudy,
            TotalAverage = x.TotalAverage,
            UniversityName = x.UniversityName
        }).ToList();
    }

    private DegreeEducationDto MappToDegreeEducation(DegreeEducation x) {
        return new DegreeEducationDto {
            User = MappToUserDto(x.User),
            CreatedAt = x.CreatedAt.ToFarsi(),
            UpdatedAt = x.UpdatedAt.ToFarsi(),
            Id = x.Id,
            UserId = x.UserId,
            GraduationYear = x.GraduationYear,
            FieldStudy = x.FieldStudy,
            TotalAverage = x.TotalAverage,
            UniversityName = x.UniversityName
        };
    }
    private UserDto MappToUserDto(User p) {
        return new UserDto {
            FullName = p.FullName,
        };
    }
}