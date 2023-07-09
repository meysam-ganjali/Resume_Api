using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class UserSkillRepository : Repository<Skill>, IUserSkillRepository {
    private readonly ResumeDbContext _context;
    public UserSkillRepository(ResumeDbContext context) : base(context) {
        _context = context;
    }

    public async Task<List<Skill>> GetUserSkills(Guid id, string include = null) {
        var skills = _context.Skills.Where(x => x.UserId == id).AsQueryable();
        if (include != null || !string.IsNullOrWhiteSpace(include))
        {
            foreach (var i in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                skills = skills.Include(i);
            }
        }
        return await skills.ToListAsync();
    }

    public async Task<Skill?> GetUserSkill(Guid skillId, Guid userId)
    {
        return await _context.Skills.FirstOrDefaultAsync(x => x.Id == skillId && x.UserId == userId);
    }
}