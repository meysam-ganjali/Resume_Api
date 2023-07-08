using Resume.Domain;
using System.Linq.Expressions;

namespace Resume.Repositories.Generic;

public interface IRepository<T> where T : BaseEntity {
    Task CreateAsync(T entity);
    Task<List<T>> GetAllAsync(int? page, int? pageSize, Expression<Func<T,bool>> filter = null, string include=null);
    Task<T?> GetAsync(Guid id, string include = null);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> filter = null);
    Task PermanentDeleteAsync(Guid id);
    public  Task<bool> UpdateAsync(T entity);
    Task SaveAsync();
}