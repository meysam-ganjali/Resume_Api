using Resume.Application.Utility;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.Application.AppInterfaces;

public interface ISkillTypeService
{
    Task<BaseResult> CreateAsync(CreateSkillTypeParam p);
    Task<BaseResult> EditAsync(UpdateSkillTypeParam p);

    Task<BaseResult<SkillTypeDto>> GetAsync(Guid id);
    Task<BaseResult<List<SkillTypeDto>>> GetAllAsync();  
    Task<BaseResult> DeleteAsync(Guid id);
}