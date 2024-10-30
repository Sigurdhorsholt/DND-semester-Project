namespace api.Controllers;
using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

public class BookingController : ControllerBase
{
    
    private readonly DatabaseConnection _dbConnection;
    private readonly IConfiguration _configuration;

    public BookingController(DatabaseConnection dbConnection, IConfiguration configuration)
    {
        _dbConnection = dbConnection;
        _configuration = configuration;
    }
    
    
    //Get specific booking
    //Get bookings for user
    //Get bookings for entire laundryroom
    //Get bookings for specific Laundry machines
    
    // 1. Get all bookings for a specific user
    [HttpGet("user/{userId}")]
    public IActionResult GetBookingsForUser(int userId)
    {

        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.userId = @userId";

        
        return ExecuteBookingQuery(query, new { userId });

  }
    
    
    
    
    
    
    // 2. Get all bookings for a specific laundry room
    [HttpGet("laundryroom/{laundryRoomId}")]
    public IActionResult GetAllBookingsForLaundryRoom(int laundryRoomId)
    {
        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.laundryRoomId = @laundryRoomId";

        return ExecuteBookingQuery(query, new { laundryRoomId });
    }
    // 3. Get a specific booking by booking ID
    [HttpGet("{bookingId}")]
    public IActionResult GetBookingById(int bookingId)
    {
        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.bookingId = @bookingId";

        return ExecuteBookingQuery(query, new { bookingId });
    }

    // 4. Get all bookings for a specific machine
    [HttpGet("machine/{machineId}")]
    public IActionResult GetBookingsForMachine(int machineId)
    {
        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.machineId = @machineId";

        return ExecuteBookingQuery(query, new { machineId });
    }

    // 5. Get all upcoming bookings (from today onward)
    [HttpGet("upcoming/{laundryRoomId}")]
    public IActionResult GetUpcomingBookings(int laundryRoomId)
    {
        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.bookingdate >= CURDATE()
                AND  b.laundryRoomId = @laundryRoomId
                ";

        return ExecuteBookingQuery(query, new { laundryRoomId });
    }
    
    
    
    
    // Get all bookings for a specific room and date
    [HttpGet("laundryroom/{laundryRoomId}/date/{bookingDate}")]
    public IActionResult GetBookingsForRoomAndDate(int laundryRoomId, DateTime bookingDate)
    {
        string query = @"
                SELECT b.bookingId, b.userId, b.machineId, b.bookingdate, b.laundryRoomId, u.apartment, u.FullName, s.startTime, s.endTime
                FROM booking b
                JOIN user u ON b.userId = u.id
                JOIN timeslots s ON b.timeslotId = s.timeslotId
                WHERE b.laundryRoomId = @laundryRoomId
                  AND b.bookingdate = @bookingDate";

        return ExecuteBookingQuery(query, new { laundryRoomId, bookingDate });
    }
    
    
    
    
    
    
    // Utility function to execute booking queries with parameters
    private IActionResult ExecuteBookingQuery(string query, object parameters)
    {
        try
        {
            _dbConnection.OpenConnection();

            using (var cmd = new MySqlCommand(query, _dbConnection.GetConnection()))
            {
                foreach (var property in parameters.GetType().GetProperties())
                {
                    cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters));
                }

                using (var reader = cmd.ExecuteReader())
                {
                    var bookings = new List<BookingDto>();
                    while (reader.Read())
                    {
                        bookings.Add(new BookingDto
                        {
                            BookingId = reader.GetInt32("bookingId"),
                            UserId = reader.GetInt32("userId"),
                            MachineId = reader.GetInt32("machineId"),
                            BookingDate = reader.GetDateTime("bookingdate"),
                            Apartment = reader.GetString("apartment"),
                            FullName = reader.GetString("FullName"),
                            StartTime = reader.GetTimeSpan("startTime").ToString(@"hh\:mm"), // Format as HH:mm
                            EndTime = reader.GetTimeSpan("endTime").ToString(@"hh\:mm"),     // Format as HH:mm
                            laundryRoomId = reader.GetInt32("laundryRoomId")

                        });
                    }
                    return Ok(bookings);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
        finally
        {
            _dbConnection.CloseConnection();
        }
    }
    
    
    }


public class BookingDto
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public int MachineId { get; set; }
    public DateTime BookingDate { get; set; }
    public string Apartment { get; set; }
    public string FullName { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int laundryRoomId { get; set; }
}