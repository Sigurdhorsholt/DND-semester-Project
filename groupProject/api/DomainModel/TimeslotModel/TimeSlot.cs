namespace api.DomainModel.TimeslotModel;

public class TimeSlot
{
    public string TimeSlotId { get; private set; }
    public DateTime StartTime { get; private set; }
    public TimeSpan Duration { get; private set; }
    public bool IsBooked { get; private set; }
    public string BookedByUserId { get; private set; }

    public TimeSlot(string timeSlotId, DateTime startTime, TimeSpan duration)
    {
        TimeSlotId = timeSlotId;
        StartTime = startTime;
        Duration = duration;
        IsBooked = false;
        BookedByUserId = null;
    }

    // Book the time slot
    public void BookTimeSlot(string userId)
    {
        if (!IsBooked)
        {
            IsBooked = true;
            BookedByUserId = userId;
            Console.WriteLine($"Time slot {TimeSlotId} booked by user {userId}.");
        }
        else
        {
            throw new InvalidOperationException("Time slot is already booked.");
        }
    }

    // Cancel the booking
    public void CancelBooking()
    {
        if (IsBooked)
        {
            IsBooked = false;
            BookedByUserId = null;
            Console.WriteLine($"Time slot {TimeSlotId} booking has been canceled.");
        }
        else
        {
            throw new InvalidOperationException("Time slot is not currently booked.");
        }
    }

    // Check if time slot overlaps with another
    public bool OverlapsWith(TimeSlot other)
    {
        return StartTime < other.StartTime + other.Duration && other.StartTime < StartTime + Duration;
    }
}
