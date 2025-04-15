using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Stellar.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseOutline> CourseOutlines { get; set; }

    public virtual DbSet<Degree> Degrees { get; set; }

    public virtual DbSet<LearningOutcome> LearningOutcomes { get; set; }

    public virtual DbSet<LearningStep> LearningSteps { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<ProgramCourse> ProgramCourses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=stellar;User Id=sa;Password=!Long2711997;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__course__3213E83F9577B28B");

            entity.ToTable("course");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Units).HasColumnName("units");
        });

        modelBuilder.Entity<CourseOutline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__course_o__3213E83FC81AFCF1");

            entity.ToTable("course_outline");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcademicChairApproval)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("academic_chair_approval");
            entity.Property(e => e.AcademicYear)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("academic_year");
            entity.Property(e => e.ApprovedByAcademicChairDate).HasColumnName("approved_by_academic_chair_date");
            entity.Property(e => e.ApprovedByAcademicChairUserId).HasColumnName("approved_by_academic_chair_user_id");
            entity.Property(e => e.ApprovedByProgramHeadDate).HasColumnName("approved_by_program_head_date");
            entity.Property(e => e.ApprovedByProgramHeadUserId).HasColumnName("approved_by_program_head_user_id");
            entity.Property(e => e.CoRequisites)
                .HasColumnType("text")
                .HasColumnName("co_requisites");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.PassingGrade)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("passing_grade");
            entity.Property(e => e.PlarMethod)
                .HasColumnType("text")
                .HasColumnName("plar_method");
            entity.Property(e => e.PreRequisites)
                .HasColumnType("text")
                .HasColumnName("pre_requisites");
            entity.Property(e => e.PreparedByUserId).HasColumnName("prepared_by_user_id");
            entity.Property(e => e.PreparedDate).HasColumnName("prepared_date");
            entity.Property(e => e.ProgramCourseId).HasColumnName("program_course_id");
            entity.Property(e => e.ProgramHeadApproval)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("program_head_approval");
            entity.Property(e => e.StudentAssessment)
                .HasColumnType("text")
                .HasColumnName("student_assessment");

            entity.HasOne(d => d.ApprovedByAcademicChairUser).WithMany(p => p.CourseOutlineApprovedByAcademicChairUsers)
                .HasForeignKey(d => d.ApprovedByAcademicChairUserId)
                .HasConstraintName("FK__course_ou__appro__4D94879B");

            entity.HasOne(d => d.ApprovedByProgramHeadUser).WithMany(p => p.CourseOutlineApprovedByProgramHeadUsers)
                .HasForeignKey(d => d.ApprovedByProgramHeadUserId)
                .HasConstraintName("FK__course_ou__appro__4CA06362");

            entity.HasOne(d => d.Instructor).WithMany(p => p.CourseOutlineInstructors)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__course_ou__instr__4AB81AF0");

            entity.HasOne(d => d.PreparedByUser).WithMany(p => p.CourseOutlinePreparedByUsers)
                .HasForeignKey(d => d.PreparedByUserId)
                .HasConstraintName("FK__course_ou__prepa__4BAC3F29");

            entity.HasOne(d => d.ProgramCourse).WithMany(p => p.CourseOutlines)
                .HasForeignKey(d => d.ProgramCourseId)
                .HasConstraintName("FK__course_ou__progr__49C3F6B7");
        });

        modelBuilder.Entity<Degree>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__degree__3213E83FA0CF7A73");

            entity.ToTable("degree");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<LearningOutcome>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__learning__3213E83F762C7B19");

            entity.ToTable("learning_outcome");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseOutlineId).HasColumnName("course_outline_id");
            entity.Property(e => e.LearningActivities)
                .HasColumnType("text")
                .HasColumnName("learning_activities");
            entity.Property(e => e.OutcomeText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("outcome_text");

            entity.HasOne(d => d.CourseOutline).WithMany(p => p.LearningOutcomes)
                .HasForeignKey(d => d.CourseOutlineId)
                .HasConstraintName("FK_learning_outcome_course_outline");
        });

        modelBuilder.Entity<LearningStep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__learning__3213E83F8E24D9A6");

            entity.ToTable("learning_step");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LearningOutcomeId).HasColumnName("learning_outcome_id");
            entity.Property(e => e.LearningText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("learning_text");

            entity.HasOne(d => d.LearningOutcome).WithMany(p => p.LearningSteps)
                .HasForeignKey(d => d.LearningOutcomeId)
                .HasConstraintName("FK__learning___learn__46E78A0C");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__program__3213E83FC791988E");

            entity.ToTable("program");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DegreeId).HasColumnName("degree_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");

            entity.HasOne(d => d.Degree).WithMany(p => p.Programs)
                .HasForeignKey(d => d.DegreeId)
                .HasConstraintName("FK_ProgramDegree");

            entity.HasOne(d => d.School).WithMany(p => p.Programs)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_ProgramSchool");
        });

        modelBuilder.Entity<ProgramCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__program___3213E83F84A0BD73");

            entity.ToTable("program_course");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");

            entity.HasOne(d => d.Course).WithMany(p => p.ProgramCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__program_c__cours__412EB0B6");

            entity.HasOne(d => d.Program).WithMany(p => p.ProgramCourses)
                .HasForeignKey(d => d.ProgramId)
                .HasConstraintName("FK__program_c__progr__403A8C7D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__role__3213E83F302AFFB3");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__school__3213E83F7A6C53FC");

            entity.ToTable("school");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user__3213E83FEA611A9C");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__user__role_id__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
