using Resume.Application.Utility;
using Resume.SharedModel.Roles;

namespace Resume.Application.AppInterfaces;

public interface IRoleService
{
    Task<BaseResult> CreateAsync(CreateRole p);
    Task<BaseResult> EditAsync(UpdateRole p);
    Task<BaseResult<List<RoleDto>>> GetAllAsync();
    Task<BaseResult<RoleDto>> GetAsync(Guid id);
    Task<BaseResult> RemoveAsync(Guid id);
}