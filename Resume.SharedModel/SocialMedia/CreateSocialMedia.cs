using Microsoft.AspNetCore.Http;

namespace Resume.SharedModel.SocialMedia;

public class CreateSocialMedia {
    public string Name { get; set; }
    public IFormFile Logo { get; set; }
    public string Link { get; set; }
    public Guid UserId { get; set; }
}