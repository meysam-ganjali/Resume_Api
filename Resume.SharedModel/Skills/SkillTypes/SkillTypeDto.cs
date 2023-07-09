namespace Resume.SharedModel.Skills.SkillTypes;

public class SkillTypeDto:BaseDto
{
    public string Name { get; set; }
}

public class CreateSkillTypeParam
{
    public string Name { get; set; }
}
public class UpdateSkillTypeParam {
    public Guid Id { get; set; }
    public string? UpdatedAt { get; set; }
    public string Name { get; set; }
}