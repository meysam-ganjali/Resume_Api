using System.Linq.Expressions;
using Resume.Domain;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Interfaces;

public interface IAuthRepository:IRepository<User>
{
    Task<User?> getUser(Expression<Func<User,bool>> ex, string inc=null);
}