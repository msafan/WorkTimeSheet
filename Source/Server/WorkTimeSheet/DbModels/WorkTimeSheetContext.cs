using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class WorkTimeSheetContext : DbContext, IDbContext
    {
        public WorkTimeSheetContext()
        {
        }

        public WorkTimeSheetContext(DbContextOptions<WorkTimeSheetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CurrentWork> CurrentWorks { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public virtual DbSet<WorkLog> WorkLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-RIFVALQ2;Initial Catalog=WorkTimeSheet;User ID=sa;Password=password@1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CurrentWork>(entity =>
            {
                entity.ToTable("CurrentWork");

                entity.HasIndex(e => e.UserId, "UK_CurrnetWork_FK_ID_User")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("FK_ID_Project");

                entity.Property(e => e.UserId).HasColumnName("FK_ID_User");

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.CurrentWorks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_CurrentWork_FK_ID_Project");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.CurrentWork)
                    .HasForeignKey<CurrentWork>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrentWork_FK_ID_User");
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.ToTable("Organization");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("FK_ID_Organization");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Origanization)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_Organization");
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.ToTable("ProjectMember");

                entity.HasIndex(e => new { e.ProjectId, e.UserId }, "UK_ProjectMember_User")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectId).HasColumnName("FK_ID_Project");

                entity.Property(e => e.UserId).HasColumnName("FK_ID_User");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectMember_Project");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProjectMember_User");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExpireDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("FK_ID_User");

                entity.Property(e => e.IssueDateTime).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("RefreshToken");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UK_User_Email")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrganizationId).HasColumnName("FK_ID_Organization");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Organization");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.HasIndex(e => e.Role, "UK_UserRole_Role")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRoleMapping>(entity =>
            {
                entity.ToTable("UserRoleMapping");

                entity.HasIndex(e => new { e.UserId, e.UserRoleId }, "UK_UserRoleMapping_UserRoles")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId).HasColumnName("FK_ID_User");

                entity.Property(e => e.UserRoleId).HasColumnName("FK_ID_UserRole");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoleMappings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoleMapping_FK_ID_User");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.UserRoleMappings)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRoleMapping_FK_ID_UserRole");
            });

            modelBuilder.Entity<WorkLog>(entity =>
            {
                entity.ToTable("WorkLog");

                entity.HasIndex(e => e.StartDateTime, "IX_WorkLog_StartDateTime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.ProjectId).HasColumnName("FK_ID_Project");

                entity.Property(e => e.UserId).HasColumnName("FK_ID_User");

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.StartDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.WorkLogs)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkLog_Project");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkLog_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
