using Resume.Application.Utility;
using Resume.SharedModel.DegreeEducations;

namespace Resume.Application.AppInterfaces;

public interface IDegreeEducationService {
    Task<BaseResult> CreateAsync(CreateDegreeEducation p);
    Task<BaseResult> UpdateAsync(UpdateDegreeEducation p);
    Task<BaseResult> DeleteAsync(Guid id, Guid userId);

    Task<BaseResult<List<DegreeEducationDto>>> GetAllAsync();
    Task<BaseResult<DegreeEducationDto>> GetAsync(Guid id);
    Task<BaseResult<List<DegreeEducationDto>>> GetAllUserDegreeEducationAsync(Guid userId);
}