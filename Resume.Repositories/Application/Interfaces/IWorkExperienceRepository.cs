using Resume.Domain;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Interfaces;

public interface IWorkExperienceRepository:IRepository<WorkExperience>
{
    Task<WorkExperience?> GetUserWorkExperience(Guid id, Guid userID, string include = null);
}