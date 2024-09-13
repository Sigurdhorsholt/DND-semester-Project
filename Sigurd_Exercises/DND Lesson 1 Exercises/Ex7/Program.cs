
using Ex7;


public class Program
{
    static void Main(string[] args)
    {
        Company company = new Company();
        
        var emp1 = new FullTimeEmployee(100);
        var emp2 = new PartTimeEmployee(30, 10);
        
        company.HireNewEmployee(emp1);
        company.HireNewEmployee(emp2);
        
        Console.WriteLine(company.GetMonthlySalaryTotal());




    }
}




