using System.Data;
using System.Security.Claims;
using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace api.Controllers;



[ApiController] 
[Route("api/[controller]")] 
public class LaundryRoomController : ControllerBase
{
    private readonly DatabaseConnection _dbConnection;


    public LaundryRoomController(DatabaseConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }



    [HttpGet("user-laundry-room")]
    public IActionResult GetUserLaundryRoom()
    {
        Console.WriteLine("GetUserLaundryRoom endpoint is being hit successfully...");

        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized(new { message = "User is not authenticated." });
        }

        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "User ID not found in token." });
        }

        Console.WriteLine($"Authenticated User ID: {userId}");

        string query = "SELECT lr.* FROM semprojlaundrydb.laundryroom lr " +
                       "JOIN semprojlaundrydb.apartmentcomplex ac ON lr.complexId = ac.id " +
                       "JOIN semprojlaundrydb.livesin li ON li.complexId = ac.id " +
                       "JOIN semprojlaundrydb.user u ON li.userId = u.id " +
                       "WHERE u.id = @userId";
    
        MySqlParameter[] parameters = { new MySqlParameter("@userId", userId) };

        try
        {
            _dbConnection.OpenConnection();
            var result = _dbConnection.ExecuteQuery(query, parameters);
            _dbConnection.CloseConnection();

            if (result.Rows.Count == 0)
            {
                return NotFound(new { message = "Laundry-room not found associated with this user." });
            }

            var laundryRoomRow = result.Rows[0];

            // Map the DataRow to the DTO
            var laundryRoom = new LaundryRoomDto
            {
                Id = Convert.ToInt32(laundryRoomRow["laundryRoomId"]),
                RoomName = laundryRoomRow["roomName"].ToString(),
                ComplexId = Convert.ToInt32(laundryRoomRow["complexId"]),
                // Add other necessary fields if required
            };

            return Ok(laundryRoom);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("laundry-room/{id}")]
    public IActionResult GetTimeSlotsForLaundryRoom(int id)
    {
        string query = @"
                    SELECT 
                    t.timeslotId,
                    t.startTime,
                    t.endTime
                FROM 
                    timeslots t
                JOIN 
                    laundryroom lr ON t.complexId = lr.complexId
                WHERE 
                    lr.laundryRoomId = @id;
    ";
        MySqlParameter[] parameters = { new MySqlParameter("@id", id) };

        try
        {
            _dbConnection.OpenConnection();
            var result = _dbConnection.ExecuteQuery(query, parameters);
            _dbConnection.CloseConnection();

            if (result.Rows.Count == 0)
            {
                return NotFound(new { message = "Laundry-room not found." });
            }
        
            var timeslots = new List<SettingsController.TimeSlotDto>();

            foreach (DataRow row in result.Rows)
            {
                var timeslot = new SettingsController.TimeSlotDto
                {
                    TimeSlotId = Convert.ToInt32(row["timeslotId"]),
                    StartTime = TimeSpan.Parse(row["startTime"].ToString()).ToString(@"hh\:mm"), // Format as "hh:mm"
                    EndTime = TimeSpan.Parse(row["endTime"].ToString()).ToString(@"hh\:mm")
                };
                timeslots.Add(timeslot);
            }
        
            Console.WriteLine("timeslots before return: " + timeslots.ToString());
        
            return Ok(timeslots);
        
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
    [HttpGet("laundry-room/accessible/{userId}")]
    public IActionResult GetAccessibleLaundryRoomsAndMachines(ulong  userId)
    {
                    string query = @"SELECT
                        r.laundryRoomId,
                        r.roomName,
                        m.machineId,
                        m.machineName
                    FROM
                         laundryroom r
                    JOIN
                         laundrymachine m ON m.laundryRoomId = r.laundryRoomId
                    JOIN
                        (
                    SELECT complexId
                    FROM livesin
                    WHERE userId = @userId
                    UNION
                    SELECT complexId
                    FROM adminmanages
                    WHERE userId = @userId
                        ) accessible_complexes ON accessible_complexes.complexId = r.complexId;";

        try
        {
            _dbConnection.OpenConnection();
            MySqlParameter[] parameters = { new MySqlParameter("@userId", MySqlDbType.UInt64) { Value = userId } }; // Adjust parameter type
            DataTable result = _dbConnection.ExecuteQuery(query, parameters);
            _dbConnection.CloseConnection();

            var roomsAndMachines = result.AsEnumerable().GroupBy(row => new
            {
                RoomId = row.Field<ulong>("laundryRoomId"), // Change type to ulong
                RoomName = row.Field<string>("roomName")
            }).Select(g => new
            {
                RoomId = g.Key.RoomId,
                RoomName = g.Key.RoomName,
                Machines = g.Select(row => new
                {
                    MachineId = row.Field<ulong>("machineId"), // Change type to ulong
                    MachineName = row.Field<string>("machineName")
                }).ToList()
            }).ToList();

            return Ok(new { rooms = roomsAndMachines });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching accessible rooms and machines: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }


    
    

    public class LaundryRoomDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int ComplexId { get; set; }
        // Add other properties if necessary
    }
    
}