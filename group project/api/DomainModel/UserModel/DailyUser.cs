namespace BackendCore.DomainModel.UserModel;

public class DailyUser : SystemUser
{
 
    public string ApartmentNumber { get; private set; }

    public DailyUser(string userId, string userName, string email, string password, string fullName, string apartmentNumber)
        : base(userId, userName, email, password, fullName)
    {
        this.ApartmentNumber = apartmentNumber;
    }

    // Method for daily user to book a laundry slot
    public void BookTimeSlot(string timeSlotId)
    {
        // Logic to book time slot
        Console.WriteLine($"Time slot {timeSlotId} booked by {UserName} (Apartment {ApartmentNumber})");
    }

    // Method for daily user to cancel a booking
    public void CancelBooking(string bookingId)
    {
        // Logic to cancel the booking
        Console.WriteLine($"Booking {bookingId} canceled by {UserName} (Apartment {ApartmentNumber})");
    }

    // Method to view current bookings
    public void ViewCurrentBookings()
    {
        // Logic to view current bookings
        Console.WriteLine($"Current bookings for {UserName} (Apartment {ApartmentNumber})");
    }
    
}