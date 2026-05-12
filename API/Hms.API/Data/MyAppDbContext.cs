using System;
using System.Collections.Generic;
using Hms.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hms.API.Data;

public partial class MyAppDbContext : DbContext
{
    public MyAppDbContext()
    {
    }

    public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AffiliatedWith> AffiliatedWiths { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Medication> Medications { get; set; }

    public virtual DbSet<Nurse> Nurses { get; set; }

    public virtual DbSet<OnCall> OnCalls { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Prescribe> Prescribes { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Stay> Stays { get; set; }

    public virtual DbSet<TrainedIn> TrainedIns { get; set; }

    public virtual DbSet<Undergo> Undergoes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=hms;User Id=sa;Password=StrongPassword@123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AffiliatedWith>(entity =>
        {
            entity.HasKey(e => new { e.Physician, e.Department }).HasName("pk_Affiliated_With");

            entity.ToTable("Affiliated_With");

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.AffiliatedWiths)
                .HasForeignKey(d => d.Department)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Affiliated_With_Department_DepartmentID");

            entity.HasOne(d => d.PhysicianNavigation).WithMany(p => p.AffiliatedWiths)
                .HasForeignKey(d => d.Physician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Affiliated_With_Physician_EmployeeID");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("pk_Appointment");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId)
                .ValueGeneratedNever()
                .HasColumnName("AppointmentID");
            entity.Property(e => e.Endo).HasColumnType("datetime");
            entity.Property(e => e.ExaminationRoom).IsUnicode(false);
            entity.Property(e => e.Starto).HasColumnType("datetime");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Appointment_Patient_SSN");

            entity.HasOne(d => d.PhysicianNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Physician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Appointment_Physician_EmployeeID");

            entity.HasOne(d => d.PrepNurseNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PrepNurse)
                .HasConstraintName("fk_Appointment_Nurse_EmployeeID");
        });

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => new { e.BlockFloor, e.BlockCode }).HasName("pk_Block");

            entity.ToTable("Block");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("pk_Department");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.HeadNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.Head)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Department_Physician_EmployeeID");
        });

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("pk_Medication");

            entity.ToTable("Medication");

            entity.Property(e => e.Code).ValueGeneratedNever();
            entity.Property(e => e.Brand)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nurse>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk_Nurse");

            entity.ToTable("Nurse");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Ssn).HasColumnName("SSN");
        });

        modelBuilder.Entity<OnCall>(entity =>
        {
            entity.HasKey(e => new { e.Nurse, e.BlockFloor, e.BlockCode, e.OnCallStart, e.OnCallEnd }).HasName("pk_On_Call");

            entity.ToTable("On_Call");

            entity.Property(e => e.OnCallStart).HasColumnType("datetime");
            entity.Property(e => e.OnCallEnd).HasColumnType("datetime");

            entity.HasOne(d => d.NurseNavigation).WithMany(p => p.OnCalls)
                .HasForeignKey(d => d.Nurse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OnCall_Nurse_EmployeeID");

            entity.HasOne(d => d.Block).WithMany(p => p.OnCalls)
                .HasForeignKey(d => new { d.BlockFloor, d.BlockCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_OnCall_Block");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Ssn).HasName("pk_Patient");

            entity.ToTable("Patient");

            entity.Property(e => e.Ssn)
                .ValueGeneratedNever()
                .HasColumnName("SSN");
            entity.Property(e => e.Address)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.InsuranceId).HasColumnName("InsuranceID");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Pcp).HasColumnName("PCP");
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.PcpNavigation).WithMany(p => p.Patients)
                .HasForeignKey(d => d.Pcp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Patient_Physician_EmployeeID");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("pk_physician");

            entity.ToTable("Physician");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Position)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Ssn).HasColumnName("SSN");
        });

        modelBuilder.Entity<Prescribe>(entity =>
        {
            entity.HasKey(e => new { e.Physician, e.Patient, e.Medication, e.Date }).HasName("pk_Prescribes");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Dose)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.AppointmentNavigation).WithMany(p => p.Prescribes)
                .HasForeignKey(d => d.Appointment)
                .HasConstraintName("fk_Prescribes_Appointment_AppointmentID");

            entity.HasOne(d => d.MedicationNavigation).WithMany(p => p.Prescribes)
                .HasForeignKey(d => d.Medication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Prescribes_Medication_Code");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Prescribes)
                .HasForeignKey(d => d.Patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Prescribes_Patient_SSN");

            entity.HasOne(d => d.PhysicianNavigation).WithMany(p => p.Prescribes)
                .HasForeignKey(d => d.Physician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Prescribes_Physician_EmployeeID");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("pk_Procedures");

            entity.Property(e => e.Code).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomNumber).HasName("pk_Room");

            entity.ToTable("Room");

            entity.Property(e => e.RoomNumber).ValueGeneratedNever();
            entity.Property(e => e.RoomType)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Block).WithMany(p => p.Rooms)
                .HasForeignKey(d => new { d.BlockFloor, d.BlockCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Room_Block_PK");
        });

        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasKey(e => e.StayId).HasName("pk_Stay");

            entity.ToTable("Stay");

            entity.Property(e => e.StayId)
                .ValueGeneratedNever()
                .HasColumnName("StayID");
            entity.Property(e => e.StayEnd).HasColumnType("datetime");
            entity.Property(e => e.StayStart).HasColumnType("datetime");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Stays)
                .HasForeignKey(d => d.Patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Stay_Patient_SSN");

            entity.HasOne(d => d.RoomNavigation).WithMany(p => p.Stays)
                .HasForeignKey(d => d.Room)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Stay_Room_Number");
        });

        modelBuilder.Entity<TrainedIn>(entity =>
        {
            entity.HasKey(e => new { e.Physician, e.Treatment }).HasName("pk_Trained_In");

            entity.ToTable("Trained_In");

            entity.Property(e => e.CertificationDate).HasColumnType("datetime");
            entity.Property(e => e.CertificationExpires).HasColumnType("datetime");

            entity.HasOne(d => d.PhysicianNavigation).WithMany(p => p.TrainedIns)
                .HasForeignKey(d => d.Physician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Trained_In_Physician_EmployeeID");

            entity.HasOne(d => d.TreatmentNavigation).WithMany(p => p.TrainedIns)
                .HasForeignKey(d => d.Treatment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Trained_In_Procedures_Code");
        });

        modelBuilder.Entity<Undergo>(entity =>
        {
            entity.HasKey(e => new { e.Patient, e.Procedures, e.Stay, e.DateUndergoes }).HasName("pk_Undergoes");

            entity.Property(e => e.DateUndergoes).HasColumnType("datetime");

            entity.HasOne(d => d.AssistingNurseNavigation).WithMany(p => p.Undergos)
                .HasForeignKey(d => d.AssistingNurse)
                .HasConstraintName("fk_Undergoes_Nurse_EmployeeID");

            entity.HasOne(d => d.PatientNavigation).WithMany(p => p.Undergos)
                .HasForeignKey(d => d.Patient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Undergoes_Patient_SSN");

            entity.HasOne(d => d.PhysicianNavigation).WithMany(p => p.Undergos)
                .HasForeignKey(d => d.Physician)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Undergoes_Physician_EmployeeID");

            entity.HasOne(d => d.ProceduresNavigation).WithMany(p => p.Undergos)
                .HasForeignKey(d => d.Procedures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Undergoes_Procedures_Code");

            entity.HasOne(d => d.StayNavigation).WithMany(p => p.Undergos)
                .HasForeignKey(d => d.Stay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Undergoes_Stay_StayID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UQ_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_Username").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RefId).HasColumnName("RefID");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
