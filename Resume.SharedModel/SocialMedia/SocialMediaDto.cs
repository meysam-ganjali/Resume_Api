using System.ComponentModel.DataAnnotations.Schema;
using Resume.SharedModel.Auth;

namespace Resume.SharedModel.SocialMedia;

public class SocialMediaDto : BaseDto {
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Link { get; set; }
    public Guid UserId { get; set; }
    public UserDto UserDto { get; set; }
}