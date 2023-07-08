using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Resume.Domain;

namespace Resume.Repositories.Generic;

public class Repository<T> : IRepository<T> where T : BaseEntity {
    private readonly DbContext _context;

    public Repository(DbContext context) {
        _context = context;
    }

    public async Task CreateAsync(T entity) {
        var res = await _context.AddAsync(entity);

    }
    public async Task<List<T>> GetAllAsync(int? page, int? pageSize, Expression<Func<T, bool>> filter = null, string include = null) {
        IQueryable<T> query = _context.Set<T>().AsQueryable();
        if (page.HasValue && pageSize.HasValue)
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        if (include != null || !string.IsNullOrWhiteSpace(include)) {

            foreach (var includeProp in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProp);
            }
        }

        if (filter != null)
            query = query.Where(filter);
        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Guid id, string include = null) {
        IQueryable<T> query = _context.Set<T>().Where(x => x.Id == id);
        if (include != null || !string.IsNullOrWhiteSpace(include))
            foreach (var includeProp in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProp);
            }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<bool> IsExistAsync(Expression<Func<T, bool>> filter = null) {
        return await _context.Set<T>().AnyAsync(filter);
    }

    public async Task PermanentDeleteAsync(Guid id) {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
            _context.Set<T>().Remove(entity);
    }

    public async Task<bool> UpdateAsync(T entity) {
        try {

            _context.Entry(entity).State = EntityState.Modified;
            return true;
        } catch {
            return false;
        }
    }

    public async Task SaveAsync() {
        await _context.SaveChangesAsync();
    }
}