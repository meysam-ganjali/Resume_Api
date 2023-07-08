namespace Resume.Domain;

public class SkillType:BaseEntity
{
    public string Name { get; set; }
    public List<Skill> Skills { get; set; }
}