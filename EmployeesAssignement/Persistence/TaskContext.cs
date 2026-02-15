using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using EmployeesAssignement.Configuration;
using EmployeesAssignement.Models;

namespace EmployeesAssignement.Persistence;

[DbConfigurationType(typeof(NpgsqlConfiguration))]
public class TaskContext : DbContext
{
    public TaskContext() : base(GetConnectionString())
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        modelBuilder.Entity<Employee>()
            .ToTable("employees")
            .HasKey(e => e.EmployeeId);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeId)
            .HasColumnName("employeeid")
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeName)
            .HasColumnName("employeename")
            .HasMaxLength(128);

        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeSalary)
            .HasColumnName("employeesalary")
            .IsRequired();

        base.OnModelCreating(modelBuilder);
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