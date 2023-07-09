using Resume.Application.Utility;
using Resume.SharedModel.Skills;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.Application.AppInterfaces;

public interface IUserSkillService
{
    Task<BaseResult<List<SkillDto>>> GetAllAsync();
    Task<BaseResult<List<SkillDto>>> GetAllUserSkillAsync(Guid userId);
    Task<BaseResult<SkillDto>> GetAsync(Guid id);
    Task<BaseResult> CreateAsync(CreateSkillParam p);
    Task<BaseResult> EditAsync(UpdateSkillParam p);
    Task<BaseResult> DeleteAsync(Guid id, Guid userId);
}