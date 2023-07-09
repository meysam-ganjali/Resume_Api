using Resume.Domain;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Interfaces;

public interface IUserSkillRepository:IRepository<Skill>
{
    Task<List<Skill>> GetUserSkills(Guid id,string include = null);
    Task<Skill?> GetUserSkill(Guid skillId,Guid  userId);
}