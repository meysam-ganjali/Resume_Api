using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class WorkExperienceRepository : Repository<WorkExperience>, IWorkExperienceRepository {
    private readonly ResumeDbContext _context;
    public WorkExperienceRepository(ResumeDbContext context) : base(context) {
        _context = context;
    }

    public async Task<WorkExperience?> GetUserWorkExperience(Guid id, Guid userID, string include = null) {
        var userWorkExperience = _context.WorkExperiences.Where(x => x.Id == id && x.UserId == userID);
        if (include != null || !string.IsNullOrWhiteSpace(include)) {
            foreach (var i in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                userWorkExperience = userWorkExperience.Include(i);
            }
        }

        return await userWorkExperience.FirstOrDefaultAsync();
    }
}