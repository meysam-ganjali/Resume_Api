using System.ComponentModel.DataAnnotations.Schema;
using Resume.SharedModel.Auth;
using Resume.SharedModel.Skills.SkillTypes;

namespace Resume.SharedModel.Skills;

public class SkillDto:BaseDto
{
    public string SkillName { get; set; }
    public int SkillPercentage { get; set; }
    public string SkillLogo { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
    public Guid SkillTypeId { get; set; }
    public SkillTypeDto SkillType { get; set; }
}