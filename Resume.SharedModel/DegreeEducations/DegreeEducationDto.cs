using System.ComponentModel.DataAnnotations.Schema;
using Resume.SharedModel.Auth;

namespace Resume.SharedModel.DegreeEducations;

public class DegreeEducationDto : BaseDto {
    public string UniversityName { get; set; }
    public int GraduationYear { get; set; }
    public float TotalAverage { get; set; }
    public string FieldStudy { get; set; }
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
}