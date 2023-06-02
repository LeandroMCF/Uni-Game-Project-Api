using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Uni.Project.WebApi.Domain;

namespace Uni.Project.WebApi.Context;

public partial class UniContext : DbContext
{
    public UniContext()
    {
    }

    public UniContext(DbContextOptions<UniContext> options)
        : base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-40Q5SCO;Database=Project_Uni;Trusted_Connection=True;TrustServerCertificate=True;");

    public DbSet<UserDomain> Users { get; set; }
    public DbSet<EvaluationDomain> Evaluations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //User table
         modelBuilder.Entity<UserDomain>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Name)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Email)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Password)
            .HasMaxLength(150)
            .IsRequired();

        modelBuilder.Entity<EvaluationDomain>()
            .Property(u => u.UpdateTime)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.CreationTime)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Status)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Permission)
            .IsRequired();

        modelBuilder.Entity<UserDomain>()
            .Property(u => u.Download)
            .HasDefaultValue(false);

        //Evaluation

        modelBuilder.Entity<EvaluationDomain>()
            .HasKey(u => u.EvaluationId);

        modelBuilder.Entity<EvaluationDomain>()
            .HasOne(m => m.UserIdNavigation)
            .WithMany(m => m.Evaluations)
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<EvaluationDomain>()
            .Property(u => u.CreationTime)
            .IsRequired();

        modelBuilder.Entity<EvaluationDomain>()
            .Property(u => u.UpdateTime)
            .IsRequired();

        modelBuilder.Entity<EvaluationDomain>()
            .Property(u => u.Score)
            .IsRequired();

        modelBuilder.Entity<EvaluationDomain>()
            .Property(u => u.Description)
            .IsRequired();


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
