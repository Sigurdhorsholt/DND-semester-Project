namespace Ex7;

public class PartTimeEmployee : Employee
{
    
    public double HourlyWage { get; set; }
    public int HoursPerMonth { get; set; }

    public PartTimeEmployee(int hoursPerMonth, double hourlyWage)
    {
        HourlyWage = hourlyWage;
        HoursPerMonth = hoursPerMonth;
        
    }
    
    public override double GetMonthlySalary()
    {
        return HourlyWage * HoursPerMonth;
    }
}