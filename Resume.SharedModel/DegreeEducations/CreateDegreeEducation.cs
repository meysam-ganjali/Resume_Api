namespace Resume.SharedModel.DegreeEducations;

public class CreateDegreeEducation {
    public string UniversityName { get; set; }
    public int GraduationYear { get; set; }
    public float TotalAverage { get; set; }
    public string FieldStudy { get; set; }
    public Guid UserId { get; set; }
}