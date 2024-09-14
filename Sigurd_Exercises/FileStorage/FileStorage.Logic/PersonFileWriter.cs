using System.Text.Json;

namespace FileStorage.Logic
{
    public class PersonFileWriter
    {
        public async Task WriteToFileAsync(List<Person> persons)
        {
            try
            {
                var folderPath = "./FileStorage/JsonFiles";
                var filePath = Path.Combine(folderPath, "persons.json");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string jsonPersons = JsonSerializer.Serialize(persons, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(filePath, jsonPersons);

                Console.WriteLine($"File written successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task<List<Person>> DeserializeFromFileAsync()
        {
            try
            {
                var folderPath = "./FileStorage/JsonFiles";
                var filePath = Path.Combine(folderPath, "persons.json");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Error: File not found at {filePath}");
                    return new List<Person>();
                }

                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<List<Person>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Person>();
            }
        }
    }
}