namespace Ex7;

public class Company
{
    
    public List<Employee> Employees { get; set; } = new List<Employee>();
    
    public double GetMonthlySalaryTotal()
    {
        var count = 0.0;
        foreach (var emp in Employees)
        {
            count += emp.GetMonthlySalary();
        }
        
        return count;

    }

    public void HireNewEmployee(Employee employee)
    {
        Employees.Add(employee);
    }
}