using System.Data.Entity;
using EmployeesAssignement.Models;
using EmployeesAssignement.Persistence;

namespace EmployeesAssignement.Services;

public class EmployeeService
{
    private readonly TaskContext _database;

    public EmployeeService(TaskContext database)
    {
        _database = database;
    }

    public async Task<Employee?> GetEmployeeAsync(int employeeId)
    {
        var employee = await _database.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
        if (employee is not null)
        {
            return employee;
        }
        
        Console.WriteLine($"Employee with ID {employeeId} not found");
        return null;
    }

    public async Task SetEmployeeAsync(int employeeId, string name, int salary)
    {
        var existingEmployee = await GetEmployeeAsync(employeeId);
        if (existingEmployee is not null)
        {
            throw new InvalidOperationException($"Employee with ID {employeeId} already exists.");
        }
        
        var newEmployee = new Employee
        {
            EmployeeId = employeeId,
            EmployeeName = name,
            EmployeeSalary = salary
        };
        
        _database.Employees.Add(newEmployee);
        await _database.SaveChangesAsync();
    }
}