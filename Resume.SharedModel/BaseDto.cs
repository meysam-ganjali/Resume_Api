namespace Resume.SharedModel;

public class BaseDto
{
    public Guid Id { get; set; }
    public string CreatedAt { get; set; }
    public string? UpdatedAt { get; set; }
}