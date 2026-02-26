using EmployeesAssignement.Models;
using EmployeesAssignement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeesAssignement.Services;

public interface IEmployeeService
{
    Task<Employee?> GetEmployeeAsync(int employeeId);
    Task SetEmployeeAsync(int employeeId, string name, int salary);
}

public class EmployeeService : IEmployeeService
{
    private readonly TaskDbContext _database;

    public EmployeeService(TaskDbContext database)
    {
        _database = database;
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        return await _database.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
    }

    public async Task SetEmployeeAsync(int employeeId, string name, int salary)
    {
        try
        {
            _database.Employees.Add(new Employee
            {
                EmployeeId = employeeId,
                EmployeeName = name,
                EmployeeSalary = salary
            });

            await _database.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting employee: {ex.Message}");
            throw;
        }
    }
}