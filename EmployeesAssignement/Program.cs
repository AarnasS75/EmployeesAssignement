using EmployeesAssignement.Persistence;
using EmployeesAssignement.Services;

using var context = new TaskContext();
var employeeService = new EmployeeService(context);

if (args.Length == 0)
{
    Console.WriteLine("Please provide a command (set-employee or get-employee).");
    return;
}

var command = args[0];

switch (command)
{
    case "set-employee":
        await HandleSetEmployee(args, employeeService);
        break;

    case "get-employee":
        await HandleGetEmployee(args, employeeService);
        break;

    default:
        Console.WriteLine("Unknown command.");
        break;
}

return;

async Task HandleSetEmployee(string[] args, EmployeeService service)
{
    try 
    {
        var idRaw = GetArgValue(args, "--employeeId");
        var name = GetArgValue(args, "--employeeName");
        var salaryRaw = GetArgValue(args, "--employeeSalary");

        if (string.IsNullOrEmpty(idRaw) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(salaryRaw))
        {
            throw new ArgumentException("Missing required arguments. Ensure --employeeId, --employeeName, and --employeeSalary are passed.");
        }

        var id = int.Parse(idRaw);
        var salary = int.Parse(salaryRaw);

        await service.SetEmployeeAsync(id, name, salary);
        Console.WriteLine($"Successfully set employee: {name} (ID: {id})");
    }
    catch (FormatException)
    {
        Console.WriteLine("Input Error: Employee ID and Salary must be numeric integers.");
        throw;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Argument Error: {ex.Message}");
        throw;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Process Error: {ex.Message}");
        throw;
    }
}

async Task HandleGetEmployee(string[] args, EmployeeService service)
{
    try 
    {
        var idRaw = GetArgValue(args, "--employeeId");

        if (string.IsNullOrEmpty(idRaw))
        {
            throw new ArgumentException("Command requires Id.");
        }

        var id = int.Parse(idRaw);
        var emp = await service.GetEmployeeAsync(id);
        
        if (emp != null)
        {
            Console.WriteLine($"[RESULT] ID: {emp.EmployeeId} | Name: {emp.EmployeeName} | Salary: {emp.EmployeeSalary}");
        }
        else
        {
            Console.WriteLine($"Search Error: No employee found with ID {id}.");
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Input Error: The provided Id is not a valid number.");
        throw;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        throw;
    }
}

string GetArgValue(string[] args, string key) 
{
    var index = Array.IndexOf(args, key);
    return index != -1 && index + 1 < args.Length ? args[index + 1] : string.Empty;
}