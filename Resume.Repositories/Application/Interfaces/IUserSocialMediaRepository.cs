using Resume.Domain;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Interfaces;

public interface IUserSocialMediaRepository:IRepository<UsersocialMedia>
{
    Task<UsersocialMedia?> GetUserSocial(Guid id, Guid userId,string include=null);
}