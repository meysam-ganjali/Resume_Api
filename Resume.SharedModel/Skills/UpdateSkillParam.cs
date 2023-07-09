using Microsoft.AspNetCore.Http;

namespace Resume.SharedModel.Skills;

public class UpdateSkillParam {
    public Guid Id { get; set; }
    public DateTime UpdateDat { get; set; }
    public string SkillName { get; set; }
    public int SkillPercentage { get; set; }
    public IFormFile? SkillLogo { get; set; }
    public Guid UserId { get; set; }
    public Guid SkillTypeId { get; set; }
}