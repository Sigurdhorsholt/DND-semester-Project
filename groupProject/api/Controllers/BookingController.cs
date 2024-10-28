namespace api.Controllers;
using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

public class BookingController
{
    
    
    
    
    
    [HttpGet("api/bookings")]
    public IActionResult GetBookings(int laundryRoomId, DateTime bookingDate)
    {
        // SQL query to get all bookings for the specified laundryRoomId and bookingDate
        string query = @"
        SELECT b.bookingId, b.userId, b.machineId, b.bookingTimeStart, b.bookingTimeEnd, b.bookingdate, u.apartment
        FROM booking b
        JOIN user u ON b.userId = u.id
        WHERE b.bookingdate = @bookingDate AND b.machineId IN (
            SELECT machineId FROM laundrymachine WHERE laundryRoomId = @laundryRoomId
        )";
    
        // Execute query and return results as JSON
    }

    [HttpPost("api/bookings")]
    public IActionResult BookSlot([FromBody] BookingDto booking)
    {
        // Insert new booking into the database
        string query = @"
        INSERT INTO booking (userId, machineId, bookingTimeStart, bookingTimeEnd, bookingdate, bookondate)
        VALUES (@userId, @machineId, @bookingTimeStart, @bookingTimeEnd, @bookingdate, NOW())";

        // Execute query and return success response
    }

    
}