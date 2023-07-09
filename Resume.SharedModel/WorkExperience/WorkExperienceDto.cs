using System.ComponentModel.DataAnnotations.Schema;
using Resume.SharedModel.Auth;

namespace Resume.SharedModel.WorkExperience;

public class WorkExperienceDto:BaseDto
{
    public string JobPossition { get; set; }
    public string Experience { get; set; }
    public string CompanyName { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
}