using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace Cortex.Module.Issues.Infrastructure.Persistence
{
    public class IssuesDbContext:DbContext
    {
        public IssuesDbContext (DbContextOptions<IssuesDbContext> options)
            :base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<WorkSpaceMember> WorkSpaceMembers { get; set; }
        public DbSet<WorkSpaceRole> WorkSpaceRoles { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkSpaceRole>().HasData(
     new WorkSpaceRole
    {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        Name = "TeamLead",
        Description = "Manages the workspace, projects, and members."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
        Name = "Backend Developer",
        Description = "Builds and maintains server-side application logic."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Name = "Frontend Developer",
        Description = "Builds and maintains user-facing interfaces."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
        Name = "Full Stack Developer",
        Description = "Works on both server-side and client-side development."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
        Name = "Mobile Developer",
        Description = "Builds and maintains iOS/Android mobile applications."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
        Name = "QA",
        Description = "Handles testing and quality assurance processes."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
        Name = "DevOps",
        Description = "Manages infrastructure, deployment, and CI/CD pipelines."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
        Name = "BA",
        Description = "Handles business analysis and requirements management."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
        Name = "UI/UX Designer",
        Description = "Designs user experience and interface layouts."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
        Name = "Graphic Designer",
        Description = "Produces visual designs and branding materials."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
        Name = "Web Designer",
        Description = "Designs website visual layout and styling."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
        Name = "Marketing Specialist",
        Description = "Manages marketing strategy and campaigns."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
        Name = "Product Manager",
        Description = "Owns product strategy and roadmap management."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
        Name = "Data Analyst",
        Description = "Handles data analysis and reporting."
    },
    new WorkSpaceRole
    {
        Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
        Name = "Security Engineer",
        Description = "Handles security audits and risk management."
    }
);


            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(1000);

                entity.HasOne(p => p.Workspace)
                    .WithMany(w => w.Projects)
                    .HasForeignKey(p => p.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(pm => pm.Id);

                entity.HasOne(pm => pm.Project)
                    .WithMany(p => p.Members)
                    .HasForeignKey(pm => pm.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pm => pm.WorkspaceMember)
                    .WithMany(m => m.ProjectMemberships)
                    .HasForeignKey(pm => pm.WorkspaceMemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

          
           

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Title).HasMaxLength(250).IsRequired();
                entity.Property(i => i.Description).IsRequired();

                entity.HasOne(i => i.Project)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(i => i.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

              
                entity.HasOne(i => i.Reporter)
                    .WithMany(m => m.ReportedIssues)
                    .HasForeignKey(i => i.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict); 

                
                entity.HasOne(i => i.Assignee)
                    .WithMany(m => m.AssignedIssues)
                    .HasForeignKey(i => i.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull); 
            });
            modelBuilder.Entity<WorkSpaceMember>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.FullName).HasMaxLength(200).IsRequired();

                entity.Property(m => m.UserId).HasMaxLength(450).IsRequired();
                entity.HasIndex(m => m.UserId);

                entity.HasOne(m => m.WorkSpaceRole)
                    .WithMany(r => r.Members)
                    .HasForeignKey(m => m.WorkSpaceRolId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Workspace)
                    .WithMany(w => w.Members)
                    .HasForeignKey(m => m.WorkspaceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
