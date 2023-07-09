using Microsoft.EntityFrameworkCore;
using Resume.Data;
using Resume.Domain;
using Resume.Repositories.Application.Interfaces;
using Resume.Repositories.Generic;

namespace Resume.Repositories.Application.Imp;

public class UserSocialMediaRepository : Repository<UsersocialMedia>, IUserSocialMediaRepository {
    private readonly ResumeDbContext _context;
    public UserSocialMediaRepository(ResumeDbContext context) : base(context) {
        _context = context;
    }

    public async Task<UsersocialMedia?> GetUserSocial(Guid id, Guid userId, string include = null) {
        var socialMedia = _context.UsersocialMediae.Where(x => x.Id == id && x.UserId == userId);
        if (include != null || !string.IsNullOrWhiteSpace(include))
            foreach (var i in include.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                socialMedia = socialMedia.Include(i);
            }

        return await socialMedia.FirstOrDefaultAsync();
    }
}