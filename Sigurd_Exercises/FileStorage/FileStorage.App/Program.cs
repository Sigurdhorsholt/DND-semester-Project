using FileStorage.Logic;

class Program
{
    static async Task Main(string[] args)
    {
        List<Person> persons =
        [
            new Person
            {
                FirstName = "John",
                LastName = "Hitler",
                Age = 30,
                Gender = 'M',
                Height = 178.2,
                Hobbies = new List<string> { "fishing", "cage fight" },
                Married = true
            },

            new Person
            {
                FirstName = "Sarah",
                LastName = "Connor",
                Age = 28,
                Gender = 'F',
                Height = 165.5,
                Hobbies = new List<string> { "hiking", "reading", "robotics" },
                Married = false
            }
        ];

        var personFileWriter = new PersonFileWriter();

        // Write persons to file
        await personFileWriter.WriteToFileAsync(persons);

        // Read persons from file
        List<Person> personsDeserialized = await personFileWriter.DeserializeFromFileAsync();

        // Display the deserialized persons
        foreach (var person in personsDeserialized)
        {
            Console.WriteLine($"{person.FirstName} {person.LastName}, Age: {person.Age}, Married: {person.Married}");
        }
    }
}