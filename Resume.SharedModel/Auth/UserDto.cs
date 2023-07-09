using System.ComponentModel.DataAnnotations.Schema;
using Resume.SharedModel.Skills;

namespace Resume.SharedModel.Auth;

public class UserDto:BaseDto
{
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Password { get; set; }
    public bool Status { get; set; }
    public string? Avatar { get; set; }
    public Guid RoleID { get; set; }
    public string Role { get; set; }

    public string? Token { get; set; }
   // public List<UsersocialMedia> UsersocialMediae { get; set; }
    public List<SkillDto> Skills { get; set; }
    //public List<DegreeEducation> DegreeEducations { get; set; }
    //public List<WorkExperience> WorkExperiences { get; set; }
}