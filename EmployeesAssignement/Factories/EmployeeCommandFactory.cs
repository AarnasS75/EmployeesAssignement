using System.CommandLine;
using EmployeesAssignement.Constants;
using EmployeesAssignement.Services;

namespace EmployeesAssignement.Factories;

public interface IEmployeeCommandFactory
{
    RootCommand BuildRootCommand();
}

public class EmployeeCommandFactory : IEmployeeCommandFactory
{
    private readonly IEmployeeService _employeeService;

    public EmployeeCommandFactory(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public RootCommand BuildRootCommand()
    {
        var rootCommand = new RootCommand("Employee management CLI");
        rootCommand.Subcommands.Add(BuildSetEmployeeCommand());
        rootCommand.Subcommands.Add(BuildGetEmployeeCommand());
        return rootCommand;
    }

    private Command BuildSetEmployeeCommand()
    {
        var command = new Command(EmployeeCommandConstants.SetEmployee, "Create or update an employee");

        var idOption = new Option<int>(EmployeeCommandOptionsConstants.EmployeeId) { Description = "Employee ID", Required = true };
        var nameOption = new Option<string>(EmployeeCommandOptionsConstants.EmployeeName) { Description = "Employee name", Required = true };
        var salaryOption = new Option<int>(EmployeeCommandOptionsConstants.EmployeeSalary) { Description = "Employee salary", Required = true };

        command.Options.Add(idOption);
        command.Options.Add(nameOption);
        command.Options.Add(salaryOption);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var id = parseResult.GetValue(idOption);
            var name = parseResult.GetValue(nameOption);
            var salary = parseResult.GetValue(salaryOption);

            try
            {
                await _employeeService.SetEmployeeAsync(id, name!, salary);
                Console.WriteLine($"Successfully set employee: {name} (ID: {id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting employee: {ex.Message}");
            }
        });

        return command;
    }

    private Command BuildGetEmployeeCommand()
    {
        var command = new Command(EmployeeCommandConstants.GetEmployee, "Get an employee by ID");

        var idOption = new Option<int>(EmployeeCommandOptionsConstants.EmployeeId) { Description = "Employee ID", Required = true };
        command.Options.Add(idOption);

        command.SetAction(async (parseResult, cancellationToken) =>
        {
            var id = parseResult.GetValue(idOption);
            var emp = await _employeeService.GetEmployeeAsync(id);

            if (emp is not null)
            {
                Console.WriteLine(
                    $"Successfully get Employee: ID: {emp.EmployeeId} | Name: {emp.EmployeeName} | Salary: {emp.EmployeeSalary}");
            }
            else
            {
                Console.WriteLine($"No employee found with ID {id}.");
            }
        });

        return command;
    }
}