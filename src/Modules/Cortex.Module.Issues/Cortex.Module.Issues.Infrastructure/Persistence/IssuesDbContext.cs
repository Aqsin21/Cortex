using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(1000);
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

          
            modelBuilder.Entity<WorkSpaceMember>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.FullName).HasMaxLength(200).IsRequired();

                entity.HasOne(m => m.WorkSpaceRole)
                    .WithMany(r => r.Members)
                    .HasForeignKey(m => m.WorkSpaceRolId)
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
        }
    }
}
