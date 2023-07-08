using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class AuthRepository:Repository<User>,IAuthRepository
{
    private readonly ResumeDbContext _context;
    public AuthRepository(ResumeDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<User?> getUser(Expression<Func<User, bool>> ex, string inc = null)
    {
        var user =  _context.Users.Where(ex);
        if(inc != null)
            foreach (var i in inc.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
            {
                user = user.Include(i);
            }

        return await user.FirstOrDefaultAsync();
    }
}