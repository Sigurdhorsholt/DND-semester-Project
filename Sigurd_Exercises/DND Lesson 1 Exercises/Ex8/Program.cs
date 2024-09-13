

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(MakeAbba("Hi","Bye"));
    }





    public static string MakeAbba(string a, string b)
    {
        return $"{a}{b}{b}{a}";
    }
    
    
}