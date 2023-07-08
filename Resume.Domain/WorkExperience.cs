using System.ComponentModel.DataAnnotations.Schema;

namespace Resume.Domain;

public class WorkExperience : BaseEntity
{
    public string JobPossition { get; set; }
    public string Experience { get; set; }
    public string CompanyName { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}