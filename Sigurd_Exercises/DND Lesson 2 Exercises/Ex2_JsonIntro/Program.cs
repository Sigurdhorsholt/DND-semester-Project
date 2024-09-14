
using System.Text.Json;
using Ex2_JsonIntro;


class Program
{
    public static List<Person> persons = new List<Person>();
    private static PersonFileWriter personFileWriter;

    static async Task Main(string[] args)
{
    personFileWriter = new PersonFileWriter(); 
    Person person1 = new Person()
    {
        FirstName = "John",
        LastName = "Hitler",
        Age = 30,
        Gender = 'M',
        Height = 178.2,
        Hobbies = ["fishing", "cage fight"],
        Married = true,
    };
    Person person2 = new Person()
    {
        FirstName = "Sarah",
        LastName = "Connor",
        Age = 28,
        Gender = 'F',
        Height = 165.5,
        Hobbies = ["hiking", "reading", "robotics"],
        Married = false,
    };

    Person person3 = new Person()
    {
        FirstName = "Michael",
        LastName = "Jordan",
        Age = 35,
        Gender = 'M',
        Height = 198.1,
        Hobbies = ["basketball", "golf", "music"],
        Married = true,
    };

    Person person4 = new Person()
    {
        FirstName = "Emma",
        LastName = "Stone",
        Age = 32,
        Gender = 'F',
        Height = 170.4,
        Hobbies = ["acting", "photography", "traveling"],
        Married = false,
    };

    persons.Add(person1);
    persons.Add(person2);
    persons.Add(person3);
    persons.Add(person4);
    
    
    await personFileWriter.WriteToFile(persons);


    List<Person> personsDeserialized = await personFileWriter.DeserializeFromFile();

    
    // Display deserialized persons list here
    foreach (var person in personsDeserialized)
    {
        Console.WriteLine($"{person.FirstName} {person.LastName}, Age: {person.Age}, Married: {person.Married}");
    }
     

    // string JsonPersons = JsonSerializer.Serialize(persons,new JsonSerializerOptions { WriteIndented = true });

    // Console.WriteLine(JsonPersons);

}


}

