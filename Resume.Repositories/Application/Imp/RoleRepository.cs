using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class RoleRepository:Repository<Role>,IRoleRepository
{
    private readonly ResumeDbContext _context;

    public RoleRepository( ResumeDbContext context) : base(context)
    {
        _context = context;
    }
}