using Resume.Application.Utility;
using Resume.SharedModel.SocialMedia;

namespace Resume.Application.AppInterfaces;

public interface ISocialMediaService {
    Task<BaseResult> CreateAsync(CreateSocialMedia p);
    Task<BaseResult> EditAsync(UpdateSocialMedia p);

    Task<BaseResult> DeleteAsync(Guid id, Guid userID);

    Task<BaseResult<List<SocialMediaDto>>> GetAllAsync();
    Task<BaseResult<List<SocialMediaDto>>> GetAllUserSocialAsync(Guid userID);
    Task<BaseResult<SocialMediaDto>> GetAsync(Guid id);
}