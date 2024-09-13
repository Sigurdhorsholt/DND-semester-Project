namespace Ex7;

public class FullTimeEmployee : Employee
{
    public double MonthlySalay { get; set; }


    public FullTimeEmployee(double monthlySalay)
    {
        MonthlySalay = monthlySalay;
    }


    public override double GetMonthlySalary()
    {
        return MonthlySalay;
    }
}