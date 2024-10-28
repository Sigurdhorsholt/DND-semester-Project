namespace BackendCore.DomainModel.UserModel;

public class SystemAdminUser : SystemUser
{
    public SystemAdminUser(string userId, string userName, string email, string password, string fullName)
        : base(userId, userName, email, password, fullName)
    {
    }

    // Method to add a new complex admin to the system
    public void AddComplexAdmin(string complexAdminId, string userName, string email, string password, string fullName)
    {
        // Logic to add complex admin (e.g., saving to the database)
        Console.WriteLine($"Complex Admin {userName} added with ID {complexAdminId}");
    }

    // Method to remove a complex admin from the system
    public void RemoveComplexAdmin(string complexAdminId)
    {
        // Logic to remove complex admin (e.g., remove from database)
        Console.WriteLine($"Complex Admin with ID {complexAdminId} removed");
    }

    // Method to configure system-wide settings (like allowed bookings)
    public void ConfigureSystemSettings(int maxBookingsPerUser)
    {
        // Logic to configure system settings
        Console.WriteLine($"System settings updated: Max bookings per user = {maxBookingsPerUser}");
    }
}