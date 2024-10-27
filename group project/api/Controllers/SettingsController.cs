using api.DataAccess.DbContext;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace api.Controllers
{
    public class SettingsController : ControllerBase
    {
        private readonly DatabaseConnection _dbConnection;
        private readonly IConfiguration _configuration;

        public SettingsController(DatabaseConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;
            _configuration = configuration;
        }

        // Save settings for a specific laundry room
        [HttpPost("api/laundry-room/{roomId}/settings")]
        public IActionResult SaveLaundryRoomSettings(int roomId, [FromBody] ComplexSettingsDto settingsDto)
        {
            Console.WriteLine("Settings saved route HIT");

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new { message = "Unauthorized save" });
            }

            try
            {
                _dbConnection.OpenConnection();  // Ensure the connection is open
                _dbConnection.BeginTransaction(); // Start transaction

                string insertOrUpdateSettingsQuery = @"
            INSERT INTO complex_settings (complexId, maxBookingsPerUser, allowShowUserInfo)
            VALUES (
                (SELECT complexId FROM laundryroom WHERE laundryRoomId = @roomId),
                @maxBookings, @allowShowUserInfo
            )
            ON DUPLICATE KEY UPDATE maxBookingsPerUser = @maxBookings, allowShowUserInfo = @allowShowUserInfo";

                MySqlParameter[] settingsParams = {
                    new MySqlParameter("@maxBookings", settingsDto.MaxBookingsPerUser),
                    new MySqlParameter("@allowShowUserInfo", settingsDto.AllowShowUserInfo), // Pass checkbox state to DB
                    new MySqlParameter("@roomId", roomId)
                };


                // Execute the query for inserting or updating settings
                _dbConnection.ExecuteNonQuery(insertOrUpdateSettingsQuery, settingsParams);

                // Handle TimeSlots and LaundryMachines (as before)
                SaveTimeSlots(roomId, settingsDto.TimeSlots);
                SaveLaundryMachines(roomId, settingsDto.LaundryMachines);

                // Commit the transaction
                _dbConnection.CommitTransaction();

                return Ok(new { message = "Settings saved successfully." });
            }
            catch (Exception e)
            {
                if (_dbConnection != null && _dbConnection != null)  // Check if connection is open before rollback
                {
                    _dbConnection.RollbackTransaction(); // Rollback transaction on error
                }
                Console.WriteLine(e);
                return StatusCode(500, new { message = "Failed to save settings.", error = e.Message });
            }
            finally
            {
                _dbConnection.CloseConnection(); // Close connection after everything is done
            }
        }




        [HttpGet("api/laundry-room/{roomId}/settings")]
        public IActionResult GetLaundryRoomSettings(int roomId)
        {
            try
            {
                _dbConnection.OpenConnection();

                string getSettingsQuery = @"
                    SELECT lr.laundryRoomId, lr.complexId, cs.maxBookingsPerUser, cs.allowShowUserInfo
                    FROM complex_settings cs
                    JOIN laundryroom lr ON cs.complexId = lr.complexId
                    WHERE lr.laundryRoomId = @roomId";
                
                MySqlParameter[] settingsParams = { new MySqlParameter("@roomId", roomId) };
                
                var settingsData = _dbConnection.ExecuteQuery(getSettingsQuery, settingsParams);
            
                if (settingsData.Rows.Count == 0)
                {
                    return NotFound(new { message = "Settings not found for this laundry room." });
                }
                
                var row = settingsData.Rows[0];
                var settings = new ComplexSettingsDto
                {
                    LaundryRoomId = Convert.ToInt32(row["laundryRoomId"]),
                    ComplexId = Convert.ToInt32(row["complexId"]),
                    MaxBookingsPerUser = Convert.ToInt32(row["maxBookingsPerUser"]),
                    AllowShowUserInfo = Convert.ToBoolean(row["allowShowUserInfo"]),
                    TimeSlots = GetTimeSlotsWithIds(roomId), // Retrieve TimeSlots with IDs
                    LaundryMachines = GetLaundryMachinesWithIds(roomId) // Retrieve Machines with IDs
                };

                _dbConnection.CloseConnection();

                return Ok(settings);  // Return settings as JSON
                
            }
            catch (Exception e)
            {
                _dbConnection.CloseConnection();
                Console.WriteLine(e);
                return StatusCode(500, new { message = "Failed to retrieve settings.", error = e.Message });
            }
        }




        // Save TimeSlots
private void SaveTimeSlots(int roomId, List<TimeSlotDto> timeSlots)
{
    string deleteTimeslotsQuery = @"
        DELETE FROM timeslots 
        WHERE complexId = (SELECT complexId FROM laundryroom WHERE laundryRoomId = @roomId)";
    
    MySqlParameter[] deleteParams = { new MySqlParameter("@roomId", roomId) };
    _dbConnection.ExecuteNonQuery(deleteTimeslotsQuery, deleteParams);

    string insertTimeslotsQuery = @"
        INSERT INTO timeslots (complexId, startTime, endTime) 
        VALUES ((SELECT complexId FROM laundryroom WHERE laundryRoomId = @roomId), @startTime, @endTime)";
    
    foreach (var slot in timeSlots)
    {
        string startTime = TimeSpan.Parse(slot.StartTime).ToString(@"hh\:mm\:ss");
        string endTime = TimeSpan.Parse(slot.EndTime).ToString(@"hh\:mm\:ss");

        MySqlParameter[] timeSlotParams = {
            new MySqlParameter("@startTime", startTime),
            new MySqlParameter("@endTime", endTime),
            new MySqlParameter("@roomId", roomId)
        };
        _dbConnection.ExecuteNonQuery(insertTimeslotsQuery, timeSlotParams);
    }
}

// Save Laundry Machines
        private void SaveLaundryMachines(int roomId, List<LaundryMachineDto> machines)
        {
            string deleteMachinesQuery = @"
        DELETE FROM laundrymachine 
        WHERE laundryRoomId = @roomId";
    
            MySqlParameter[] deleteParams = { new MySqlParameter("@roomId", roomId) };
            _dbConnection.ExecuteNonQuery(deleteMachinesQuery, deleteParams);

            string insertMachinesQuery = @"
        INSERT INTO laundrymachine (machineName, machineType, status, laundryRoomId) 
        VALUES (@machineName, @machineType, 'Available', @roomId)";
    
            foreach (var machine in machines)
            {
                // Map Danish terms to ENUM values expected by the DB
                string machineType = machine.MachineType == "Vaskemaskine" ? "Washer" : machine.MachineType == "Tørretumbler" ? "Dryer" : machine.MachineType;

                MySqlParameter[] machineParams = {
                    new MySqlParameter("@machineName", machine.MachineName),
                    new MySqlParameter("@machineType", machineType),
                    new MySqlParameter("@roomId", roomId)
                };
                _dbConnection.ExecuteNonQuery(insertMachinesQuery, machineParams);
            }
        }
        
        
        private List<LaundryMachineDto> GetLaundryMachinesWithIds(int roomId)
        {
            string query = @"
                SELECT lm.machineId, lm.machineName, lm.machineType
                FROM laundrymachine lm
                WHERE lm.laundryRoomId = @roomId";
    
            MySqlParameter[] parameters  = { new MySqlParameter("@roomId", roomId) };
            var result = _dbConnection.ExecuteQuery(query, parameters);

            var machines = new List<LaundryMachineDto>();
            foreach (DataRow row in result.Rows)
            {
                machines.Add(new LaundryMachineDto
                {
                    MachineId = int.Parse(row["machineId"].ToString()),
                    MachineName = row["machineName"].ToString(),
                    MachineType = row["machineType"].ToString()
                });
            }
            return machines;
        }

        private List<TimeSlotDto> GetTimeSlotsWithIds(int roomId)
        {
            string query = @"
                SELECT ts.timeslotId, ts.startTime, ts.endTime
                FROM timeslots ts
                JOIN laundryroom lr ON ts.complexId = lr.complexId
                WHERE lr.laundryRoomId = @roomId";
            
    
            MySqlParameter[] parameters  = { new MySqlParameter("@roomId", roomId) };
            var result = _dbConnection.ExecuteQuery(query, parameters);

            var timeSlots = new List<TimeSlotDto>();
            foreach (DataRow row in result.Rows)
            {
                timeSlots.Add(new TimeSlotDto
                {
                    TimeSlotId = int.Parse(row["timeslotId"].ToString()),
                    StartTime = TimeSpan.Parse(row["startTime"].ToString()).ToString(@"hh\:mm\:ss"),
                    EndTime = TimeSpan.Parse(row["endTime"].ToString()).ToString(@"hh\:mm\:ss")
                });
            }
            return timeSlots;        }       
        
        

        // DTO for complex settings
        public class ComplexSettingsDto
        {
            public int LaundryRoomId { get; set; } // Add laundryRoomId
            public int ComplexId { get; set; } // Add complexId
            public int MaxBookingsPerUser { get; set; } // Max bookings per user
            public List<TimeSlotDto> TimeSlots { get; set; } // List of time slots
            public List<LaundryMachineDto> LaundryMachines { get; set; } // List of laundry machines
            public bool AllowShowUserInfo { get; set; } // New field for showing user names
        }


        public class TimeSlotDto
        {
            public int TimeSlotId { get; set; } // Add timeSlotId
            public string StartTime { get; set; }
            public string EndTime { get; set; }
        }

        public class LaundryMachineDto
        {
            public int MachineId { get; set; } // Add machineId
            public string MachineName { get; set; }
            public string MachineType { get; set; }
        }
    }
}
