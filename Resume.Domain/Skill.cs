using System.ComponentModel.DataAnnotations.Schema;

namespace Resume.Domain;

public class Skill:BaseEntity
{
    public string SkillName { get; set; }
    public int SkillPercentage { get; set; }
    public string SkillLogo { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public Guid SkillTypeId { get; set; }
    [ForeignKey("SkillTypeId")]
    public SkillType SkillType { get; set; }
}