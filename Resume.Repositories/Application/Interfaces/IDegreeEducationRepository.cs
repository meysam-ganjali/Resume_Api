using Resume.Domain;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Interfaces;

public interface IDegreeEducationRepository:IRepository<DegreeEducation>
{
    Task<DegreeEducation?> GetUserDegreeEducation(Guid id, Guid userID,string include=null);
}