using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class DegreeEducationRepository : Repository<DegreeEducation>, IDegreeEducationRepository {
    private readonly ResumeDbContext _context;
    public DegreeEducationRepository(ResumeDbContext context) : base(context) {
        _context = context;
    }

    public async Task<DegreeEducation?> GetUserDegreeEducation(Guid id, Guid userID, string include = null) {
        var res = _context.DegreeEducations.Where(x => x.Id == id && x.UserId == userID);
        if (include != null || !string.IsNullOrWhiteSpace(include))
            foreach (var i in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                res = res.Include(i);
            }

        return await res.FirstOrDefaultAsync();
    }
}