using Microsoft.EntityFrameworkCore;
using Resume.Domain;

namespace Resume.Data;

public class ResumeDbContext:DbContext
{
    public ResumeDbContext(DbContextOptions<ResumeDbContext> options):base(options)
    {
        
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User?> Users { get; set; }
    public DbSet<DegreeEducation> DegreeEducations { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillType> SkillTypes { get; set; }
    public DbSet<UsersocialMedia?> UsersocialMediae { get; set; }
    public DbSet<WorkExperience?> WorkExperiences { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Role>().HasData(new List<Role>
        {
            new Role{Id = Guid.NewGuid(),CreatedAt = DateTime.Now,Name = "Administrator",UpdatedAt = null}
        });
    }
}