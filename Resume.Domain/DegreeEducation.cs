using System.ComponentModel.DataAnnotations.Schema;

namespace Resume.Domain;

public class DegreeEducation : BaseEntity {
    public string UniversityName { get; set; }
    public int GraduationYear { get; set; }
    public float TotalAverage { get; set; }
    public string FieldStudy { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}