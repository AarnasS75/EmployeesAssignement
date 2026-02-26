using EmployeesAssignement.Commands;
using EmployeesAssignement.Factories;
using EmployeesAssignement.Persistence;
using EmployeesAssignement.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddDbContext<TaskDbContext>()
    .AddScoped<IEmployeeService, EmployeeService>()
    .AddScoped<IEmployeeCommandFactory, EmployeeCommandFactory>()
    .AddScoped<IEmployeeCommandHandler, EmployeeCommandHandler>()
    .BuildServiceProvider();

var handler = services.GetRequiredService<IEmployeeCommandHandler>();
await handler.HandleAsync(args);