using AwesomeCompany.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeCompany.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var employees = new List<Employee>();
            for (int i = 0; i < 1000; i++)
            {
                employees.Add(new Employee
                {
                    Id = i + 1,
                    CompanyId = 1,
                    Name = $"Employee {i + 1}",
                    Salary = 1000m
                });
            }
            modelBuilder.Entity<Company>(builder =>
            {
                builder.ToTable("Companies");
                builder.HasMany(company => company.Employees)
                .WithOne()
                .HasForeignKey(employee => employee.CompanyId)
                .IsRequired();

                builder.HasData(new Company
                {
                    Id = 1,
                    Name = "Awesome Company",
                });
            });
            modelBuilder.Entity<Employee>(builder =>
            {
                builder.ToTable("Employees");
                builder.HasData(employees);
            });

        }
    }
}
