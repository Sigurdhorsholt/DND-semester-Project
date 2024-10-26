using api.DomainModel.TimeslotModel;

namespace api.DomainModel.LaundryRoomModel;


public enum MachineStatus
{
    Available,
    InUse,
    OutOfOrder
}

public enum MachineType
{
    Washer,
    Dryer
}


public class LaundryMachine
{
    public int MachineId { get;  set; }
    public MachineType Type { get; private set; }
    public MachineStatus Status { get; private set; }
    public TimeSlot CurrentBooking { get; private set; }

    public LaundryMachine(int machineId, MachineType type)
    {
        MachineId = machineId;
        Type = type;
        Status = MachineStatus.Available;  // Machines are available when created
        CurrentBooking = null;  // No booking initially
    }
    // Set machine to in-use status with the current booking
    public void StartBooking(TimeSlot booking)
    {
        if (Status == MachineStatus.Available)
        {
            Status = MachineStatus.InUse;
            CurrentBooking = booking;
            Console.WriteLine($"Machine {MachineId} is now in use.");
        }
        else
        {
            throw new InvalidOperationException("Machine is not available for booking.");
        }
    }

    // Release machine when the booking is completed
    public void CompleteBooking()
    {
        if (Status == MachineStatus.InUse)
        {
            Status = MachineStatus.Available;
            CurrentBooking = null;
            Console.WriteLine($"Machine {MachineId} is now available.");
        }
        else
        {
            throw new InvalidOperationException("Machine is not currently in use.");
        }
    }
    
    
    // Mark machine as out of order
    public void SetOutOfOrder()
    {
        Status = MachineStatus.OutOfOrder;
        CurrentBooking = null;  // Clear any bookings
        Console.WriteLine($"Machine {MachineId} is now out of order.");
    }


}