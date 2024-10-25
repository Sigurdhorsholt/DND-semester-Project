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


    

    [HttpGet("bookings-on-date")] //TODO : Move to other Controller
    public IActionResult GetBookingsOnDate([FromQuery] DateTime date)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Query to retrieve all bookings on the specified date
        string query = @"SELECT b.bookingId, 
                            b.userId, 
                            b.machineId, 
                            b.bookingTimeStart, 
                            b.bookingTimeEnd, 
                            b.bookingdate, 
                            b.bookondate
                     FROM semprojlaundrydb.booking b
                     WHERE b.bookingdate = @date";

        MySqlParameter[] parameters = {
            new MySqlParameter("@date", date.ToString("yyyy-MM-dd"))
        };

        try
        {
            _dbConnection.OpenConnection();
            var result = _dbConnection.ExecuteQuery(query, parameters);
            _dbConnection.CloseConnection();

            if (result.Rows.Count == 0)
            {
                return NotFound(new { message = "No bookings found for this date." });
            }

            // Assuming result is a DataTable or similar structure
            var bookings = result.AsEnumerable().Select(row => new 
            {
                BookingId = row["bookingId"],
                UserId = row["userId"],
                MachineId = row["machineId"],
                BookingTimeStart = row["bookingTimeStart"],
                BookingTimeEnd = row["bookingTimeEnd"],
                BookingDate = row["bookingdate"],
                BookOnDate = row["bookondate"]
            });

            return Ok(bookings); // Return the list of bookings
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
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