using EmployeesAssignement.Factories;

namespace EmployeesAssignement.Commands;

public interface IEmployeeCommandHandler
{
    Task HandleAsync(string[] args);
}

public class EmployeeCommandHandler : IEmployeeCommandHandler
{
    private readonly IEmployeeCommandFactory _commandFactory;

    public EmployeeCommandHandler(IEmployeeCommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public async Task HandleAsync(string[] args)
    {
        var rootCommand = _commandFactory.BuildRootCommand();
        await rootCommand.Parse(args).InvokeAsync();
    }
}