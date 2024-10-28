namespace BackendCore.DomainModel.UserModel;

public class ComplexAdminUser : SystemUser
{
    public string ComplexId { get; private set; } // Specific to a complex

    public ComplexAdminUser(string userId, string userName, string email, string password, string fullName, string complexId)
        : base(userId, userName, email, password, fullName)
    {
        this.ComplexId = complexId;
    }

    // Method to add a laundry machine to the complex
    public void AddLaundryMachine(string machineId, string machineType)
    {
        // Logic to add machine (washer or dryer) to the complex
        Console.WriteLine($"{machineType} with ID {machineId} added to complex {ComplexId}");
    }

    // Method to remove a laundry machine from the complex
    public void RemoveLaundryMachine(string machineId)
    {
        // Logic to remove machine from the complex
        Console.WriteLine($"Machine with ID {machineId} removed from complex {ComplexId}");
    }

    // Method to add time slots for laundry bookings
    public void AddTimeSlot(string timeSlotId, DateTime startTime, TimeSpan duration)
    {
        // Logic to add a time slot
        Console.WriteLine($"Time slot {timeSlotId} added for complex {ComplexId} at {startTime}");
    }

    // Method to add a new user to the complex
    public void AddUserToComplex(string userId, string userName, string email, string password, string fullName)
    {
        // Logic to add user to complex
        Console.WriteLine($"User {userName} added to complex {ComplexId}");
    }

    // Method to remove a user from the complex
    public void RemoveUserFromComplex(string userId)
    {
        // Logic to remove user from complex
        Console.WriteLine($"User with ID {userId} removed from complex {ComplexId}");
    }
}