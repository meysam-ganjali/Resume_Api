using Microsoft.AspNetCore.Http;

namespace Resume.SharedModel.SocialMedia;

public class UpdateSocialMedia {
    public string Name { get; set; }
    public IFormFile? Logo { get; set; }
    public string Link { get; set; }
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
}