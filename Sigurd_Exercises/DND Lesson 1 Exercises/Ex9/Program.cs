public class Program
{
    static void Main(string[] args)
    {
        
        Console.WriteLine(makeOutWord("<<>>", "Yay"));      // Output: "<<Yay>>"
        Console.WriteLine(makeOutWord("<<>>", "WooHoo"));   // Output: "<<WooHoo>>"
        Console.WriteLine(makeOutWord("[[]]", "word"));     // Output: "[[word]]"

    }


    public static string makeOutWord(string outer, string inner)
    {
        string front = outer.Substring(0, 2);
        string back = outer.Substring(outer.Length - 2, 2);
        
        return $"{front}{inner}{back}";

    }
}