using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class UserSkillRepository:Repository<Skill>,IUserSkillRepository
{
    private readonly ResumeDbContext _context;
    public UserSkillRepository(ResumeDbContext context) : base(context)
    {
        _context = context;
    }
}