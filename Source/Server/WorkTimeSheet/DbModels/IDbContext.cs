using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.DbModels
{
    public interface IDbContext
    {
        DbSet<CurrentWork> CurrentWorks { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectMember> ProjectMembers { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        DbSet<WorkLog> WorkLogs { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<AccessToken> AccessTokens { get; set; }

        int SaveChanges();
    }
}
