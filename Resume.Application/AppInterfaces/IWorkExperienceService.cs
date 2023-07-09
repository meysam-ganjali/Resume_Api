using Resume.Application.Utility;
using Resume.SharedModel.WorkExperience;

namespace Resume.Application.AppInterfaces;

public interface IWorkExperienceService
{
    Task<BaseResult> CreateAsync(CreateWorkExperience p);
    Task<BaseResult> EditAsync(UpdateWorkExperience p);

    Task<BaseResult<List<WorkExperienceDto>>> GetAllAsync();
    Task<BaseResult<List<WorkExperienceDto>>> GetAllUserWorkExperienceAsync(Guid userID);
    Task<BaseResult<WorkExperienceDto>> GetAsync(Guid id);

    Task<BaseResult> DeleteAsync(Guid id, Guid userId);
}