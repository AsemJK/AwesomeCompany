namespace AwesomeCompany.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public decimal Salary { get; set; }
    }
}
