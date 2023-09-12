using AwesomeCompany;
using AwesomeCompany.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(p => p.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

var sampleTodos = TodoGenerator.GenerateTodos().ToArray();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());



var employeesApi = app.MapGroup("/employee");

employeesApi.MapGet("/all", async (DatabaseContext _dbContext) =>
{
    return _dbContext.Employees.ToList();
});

employeesApi.MapGet("/{id}", async (int id, DatabaseContext _dbContext) =>
{
    return _dbContext.Employees.FirstOrDefault(v => v.Id == id);
});

employeesApi.MapPut("/increase-salaries", async (int companyId, DatabaseContext _dbContext) =>
{
    var company = _dbContext.Companies.Include(c => c.Employees).FirstOrDefault(c => c.Id == companyId);
    if (company != null)
    {
        foreach (var employee in company.Employees)
        {
            employee.Salary *= 1.1m;
        }
        company.LastSalaryUpdateUTC = DateTime.UtcNow;
        _dbContext.SaveChanges();
    }
    else
        return Results.NotFound("There is no company with this supplied ID");
    return Results.Ok();
});

app.Run();

