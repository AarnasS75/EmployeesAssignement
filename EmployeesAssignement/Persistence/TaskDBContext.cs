using EmployeesAssignement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesAssignement.Persistence;

public class TaskDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
        
        modelBuilder.Entity<Employee>()
            .ToTable("employees")
            .HasKey(e => e.EmployeeId);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeId)
            .HasColumnName("employeeid")
            .ValueGeneratedNever();

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeName)
            .HasColumnName("employeename")
            .HasMaxLength(128);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeSalary)
            .HasColumnName("employeesalary")
            .IsRequired();
    }

    private static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("connectionString");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string not found. Please set the 'connectionString' environment variable.");
        }

        return connectionString;
    }
}