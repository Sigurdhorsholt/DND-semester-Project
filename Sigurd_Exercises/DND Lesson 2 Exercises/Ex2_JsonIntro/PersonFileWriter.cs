using System.Security.Principal;
using System.Text.Json;

namespace Ex2_JsonIntro
{
    public class PersonFileWriter
    {
        public async Task WriteToFile(List<Person> persons)
        {
            try
            {
                // Find it in file explorer here:
                // C:\Users\sigur\OneDrive\Dokumenter\GitHub\DND-semester-Project\Sigurd_Exercises\DND Lesson 2 Exercises\Ex2_JsonIntro\bin\Debug\net8.0\Ex2_JsonIntro\JsonFiles
                var folderPath = "./Ex2_JsonIntro/JsonFiles";
                var filePath = Path.Combine(folderPath, "persons.json");

                // Ensure the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string jsonPersons = JsonSerializer.Serialize(persons,
                    new JsonSerializerOptions { WriteIndented = true });

               await File.WriteAllTextAsync(filePath, jsonPersons);

                Console.WriteLine($"File written successfully to: {filePath}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Error: Access to the path is denied. " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }


        public async Task<List<Person>> DeserializeFromFile()
        {

            try
            {
                var folderPath = "./Ex2_JsonIntro/JsonFiles";
                var filePath = Path.Combine(folderPath, "persons.json");
                
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: File not found at " + filePath);
                    return new List<Person>();
                }
                
                
                string json = await File.ReadAllTextAsync(filePath);
                List<Person> persons = JsonSerializer.Deserialize<List<Person>>(json);
            
                return persons;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: File not found. " + ex.Message);
                return new List<Person>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return new List<Person>();
            }
            
          
            
            
        }
        
    }
}