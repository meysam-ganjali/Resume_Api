using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.Validation;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.Application.AppImp;

public class SkillTypeService : ISkillTypeService {
    private readonly ISkillTypeRepository _skillTypeRepository;

    public SkillTypeService(ISkillTypeRepository skillTypeRepository) {
        _skillTypeRepository = skillTypeRepository;
    }
    public async Task<BaseResult> CreateAsync(CreateSkillTypeParam p) {
        if (await _skillTypeRepository.IsExistAsync(x => x.Name == p.Name))
            return new BaseResult {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = ValidationMessages.DuplicatedRecord
            };
        var skillType = new SkillType {
            CreatedAt = DateTime.Now,
            Name = p.Name,

        };
        await _skillTypeRepository.CreateAsync(skillType);
        await _skillTypeRepository.SaveAsync();
        return new BaseResult {
            StatusCode = HttpStatusCode.Created,
            IsSuccess = true,
            Message = "نوع مهارت با موفقیت ثبت شد",
        };
    }

    public async Task<BaseResult> EditAsync(UpdateSkillTypeParam p) {
        var skillType = await _skillTypeRepository.GetAsync(p.Id);
        if (skillType == null)
            return new BaseResult {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound
            };
        if (await _skillTypeRepository.IsExistAsync(x => x.Name == p.Name && x.Id != p.Id))
            return new BaseResult {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = ValidationMessages.DuplicatedRecord
            };
        skillType.Name = p.Name;
        skillType.UpdatedAt = DateTime.Now;
        await _skillTypeRepository.SaveAsync();
        return new BaseResult {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Message = ValidationMessages.SuccessfulyEdit
        };
    }

    public async Task<BaseResult<SkillTypeDto>> GetAsync(Guid id) {
        var skillType = await _skillTypeRepository.GetAsync(id);
        if (skillType == null)
            return new BaseResult<SkillTypeDto> {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound,
                Data = null
            };
        return new BaseResult<SkillTypeDto> {
            Data = MappToDto(skillType),
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
        };
    }

    private SkillTypeDto MappToDto(SkillType skillType) {
        return new SkillTypeDto() {
            CreatedAt = skillType.CreatedAt.ToFarsi(),
            Id = skillType.Id,
            Name = skillType.Name,
            UpdatedAt = skillType.UpdatedAt.ToFarsi(),
        };
    }

    public async Task<BaseResult<List<SkillTypeDto>>> GetAllAsync() {
        var skillTypes = await _skillTypeRepository.GetAllAsync(null, null);
        return new BaseResult<List<SkillTypeDto>> {
            Data = MappToSkillTypesDto(skillTypes),
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true
        };
    }

    private List<SkillTypeDto> MappToSkillTypesDto(List<SkillType> skillTypes) {
        return skillTypes.Select(x => new SkillTypeDto {
            CreatedAt = x.CreatedAt.ToFarsi(),
            Id = x.Id,
            Name = x.Name,
            UpdatedAt = x.UpdatedAt.ToFarsi(),
        }).ToList();
    }

    public async Task<BaseResult> DeleteAsync(Guid id) {
        var skillType = await _skillTypeRepository.GetAsync(id, include: "Skills");
        if (skillType == null)
            return new BaseResult {
                StatusCode = HttpStatusCode.NotFound,
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound,
            };
        if (skillType.Skills.Any())
            return new BaseResult {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = "این نوع قابل حذف نیست.",
            };
        await _skillTypeRepository.PermanentDeleteAsync(skillType.Id);
        await _skillTypeRepository.SaveAsync();
        return new BaseResult {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Message = "عملیات حذف به پایان رسید"
        };
    }
}