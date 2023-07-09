namespace Resume.SharedModel.WorkExperience;

public class UpdateWorkExperience{
    public Guid Id { get; set; }
    public string JobPossition { get; set; }
    public string Experience { get; set; }
    public string CompanyName { get; set; }
    public Guid UserId { get; set; }
}