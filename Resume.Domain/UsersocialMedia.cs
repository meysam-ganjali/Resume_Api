using System.ComponentModel.DataAnnotations.Schema;

namespace Resume.Domain;

public class UsersocialMedia:BaseEntity
{
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Link { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}