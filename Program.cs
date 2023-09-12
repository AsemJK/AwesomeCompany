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
app.Run();

