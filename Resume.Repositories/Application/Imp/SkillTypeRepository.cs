using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class SkillTypeRepository:Repository<SkillType>, ISkillTypeRepository
{
    private readonly ResumeDbContext _context;

    public SkillTypeRepository(ResumeDbContext context) : base(context)
    {
        _context = context;
    }
}