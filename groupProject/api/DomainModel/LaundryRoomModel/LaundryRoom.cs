using api.DomainModel.TimeslotModel;

namespace api.DomainModel.LaundryRoomModel;

public class LaundryRoom
{
    public string RoomId { get; private set; }
    public string ComplexId { get; private set; }  // ID of the complex this room belongs to
    public List<LaundryMachine> Machines { get; private set; }
    public List<TimeSlot> TimeSlots { get; private set; }

    public LaundryRoom(string roomId, string complexId)
    {
        RoomId = roomId;
        ComplexId = complexId;
        Machines = new List<LaundryMachine>();
        TimeSlots = new List<TimeSlot>();
    }
    
    // Add a laundry machine to the room
    public void AddMachine(LaundryMachine machine)
    {
        Machines.Add(machine);
        Console.WriteLine($"Machine {machine.MachineId} added to room {RoomId}");
    }

    // Add a time slot for booking
    public void AddTimeSlot(TimeSlot timeSlot)
    {
        TimeSlots.Add(timeSlot);
        Console.WriteLine($"Time slot {timeSlot.TimeSlotId} added to room {RoomId}");
    }

    // Retrieve all available machines
    public List<LaundryMachine> GetAvailableMachines()
    {
        return Machines.Where(m => m.Status == MachineStatus.Available).ToList();
    }
}