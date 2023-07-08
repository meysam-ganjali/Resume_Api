using System.Net;
using Framework.V1.DatetimeTool;
using Framework.V1.Validation;
using Resume.Application.AppInterfaces;
using Resume.Application.Utility;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.SharedModel.Roles;

namespace Resume.Application.AppImp;

public class RoleService : IRoleService {
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository) {
        _roleRepository = roleRepository;
    }
    public async Task<BaseResult> CreateAsync(CreateRole p) {
        if (await _roleRepository.IsExistAsync(x => x.Name == p.Name)) {
            return new BaseResult() {
                IsSuccess = false,
                Message = ValidationMessages.DuplicatedRecord,
                StatusCode = HttpStatusCode.OK
            };
        }

        var role = new Role {
            CreatedAt = DateTime.Now,
            Name = p.Name,
        };
        await _roleRepository.CreateAsync(role);
        await _roleRepository.SaveAsync();
        return new BaseResult() {
            IsSuccess = true,
            Message = "نقش مورد نظر با موفقیت ثبت شد",
            StatusCode = HttpStatusCode.Created
        };
    }

    public async Task<BaseResult> EditAsync(UpdateRole p) {
        var role = await _roleRepository.GetAsync(p.Id);
        if (await _roleRepository.IsExistAsync(x => x.Name == p.Name && x.Id != p.Id)) {
            return new BaseResult() {
                IsSuccess = false,
                Message = ValidationMessages.DuplicatedRecord,
                StatusCode = HttpStatusCode.OK
            };
        }
        if (role == null)
            return new BaseResult() {
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound,
            };
        role.Name = p.Name;
        role.UpdatedAt = DateTime.Now;
        await _roleRepository.SaveAsync();
        return new BaseResult() {
            IsSuccess = true,
            Message = ValidationMessages.SuccessfulyEdit,
            StatusCode = HttpStatusCode.OK,
        };
    }

    public async Task<BaseResult<List<RoleDto>>> GetAllAsync() {
        var roles = await _roleRepository.GetAllAsync(null, null);
        return new BaseResult<List<RoleDto>> {
            Data = MappToRolesDto(roles),
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }

    private List<RoleDto> MappToRolesDto(List<Role> roles) {
        return roles.Select(x => new RoleDto() {
            CreatedAt = x.CreatedAt.ToFarsi(),
            Id = x.Id,
            Name = x.Name,
            UpdatedAt = x.UpdatedAt.ToFarsi(),
        }).ToList();
    }

    public async Task<BaseResult<RoleDto>> GetAsync(Guid id) {
        var role = await _roleRepository.GetAsync(id);
        if (role == null)
            return new BaseResult<RoleDto>() {
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound,
                Data = null,
            };
        return new BaseResult<RoleDto> {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NotFound,
            Data = MappToDto(role),
        };
    }

    private RoleDto MappToDto(Role role) {
        return new RoleDto {
            CreatedAt = role.CreatedAt.ToFarsi(),
            Id = role.Id,
            Name = role.Name,
            UpdatedAt = role.UpdatedAt.ToFarsi(),
        };
    }

    public async Task<BaseResult> RemoveAsync(Guid id) {
        var role = await _roleRepository.GetAsync(id, include: "Users");
        if (role.Users.Any()) {
            return new BaseResult() {
                IsSuccess = false,
                Message = "این نقش قابل حذف نیست",
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
        if (role == null)
            return new BaseResult() {
                IsSuccess = false,
                Message = ValidationMessages.RecordNotFound,
                StatusCode = HttpStatusCode.NotFound,
            };
        await _roleRepository.PermanentDeleteAsync(role.Id);
        await _roleRepository.SaveAsync();
        return new BaseResult {
            IsSuccess = true,
            Message = " نقش مورد نظر حذف گردید",
            StatusCode = HttpStatusCode.OK,
        };
    }
}