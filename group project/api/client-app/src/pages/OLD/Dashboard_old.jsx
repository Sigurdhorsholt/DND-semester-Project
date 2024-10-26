

import React, { useState } from "react";
import Dateselector from "../../Components/Dateselector";
import TimeSlot from "../../Components/TimeSlot";


function Dashboard() {

    const [selectedDate, setSelectedDate] = useState(new Date());

    const bookSlot = (apartment, startTime, endTime) => {
        // Method to handle booking
        console.log(`Booking slot for ${apartment} from ${startTime} to ${endTime} on ${selectedDate}`);
    };


    return (

        <>
            <Dateselector selectedDate={selectedDate} setSelectedDate={setSelectedDate} />
            <TimeSlot
                apartment={"4TH"}
                startTime={"08:00"}
                endTime={"10:00"}
                isAvailable={true}
                selectedDate={selectedDate}
                onBookSlot={bookSlot}
            />

            <TimeSlot
                apartment={"3TH"}
                startTime={"10:00"}
                endTime={"12:00"}
                isAvailable={false}
                selectedDate={selectedDate}
                onBookSlot={bookSlot}
            />
        </>



    )


}



export default Dashboard;



/* 




import React, { useState, useEffect } from "react";
 
import axios from "axios";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

const Dashboard = () => {
    const [laundryRoom, setLaundryRoom] = useState(null);
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [timeSlots, setTimeSlots] = useState([]);
    const [error, setError] = useState("");

    // Fetch the user's laundry room on component mount
    useEffect(() => {
        const fetchLaundryRoom = async () => {
            try {
                const token = localStorage.getItem("token");

                const response = await axios.get("/api/laundryroom/user-laundry-room", {
                    headers: { Authorization: `Bearer ${token}` },
                });

                setLaundryRoom(response.data);
            } catch (error) {
                console.log("Error fetching laundry room:", error);
                setError("Unable to fetch laundry room.");
            }
        };

        fetchLaundryRoom();  // Call the fetch function
    }, []);  // Add dependency array to trigger the effect on mount

    // Fetch available times when the selected date changes
    useEffect(() => {
        if (laundryRoom && selectedDate) {
            const fetchAvailableTimes = async () => {
                try {
                    const response = await axios.get("/api/laundryroom/bookings-on-date", {
                        params: { date: selectedDate.toISOString().split("T")[0] },
                        headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
                    });
                    setTimeSlots(response.data);
                } catch (error) {
                    console.log("Error fetching available times:", error);
                    setError("Unable to fetch available times.");
                }
            };

            fetchAvailableTimes();
        }
    }, [laundryRoom, selectedDate]);

    return (
        <div>
            <h1>Dashboard</h1>
            {error && <p style={{ color: "red" }}>{error}</p>}

            {laundryRoom ? (
                <div>
                    <h2>Your Laundry Room: {laundryRoom.roomName}</h2>
                    <p>Select a date to view available times:</p>
                    <DatePicker
                        selected={selectedDate}
                        onChange={(date) => setSelectedDate(date)}
                    />

                    <h3>Available Times for {selectedDate.toDateString()}:</h3>
                    {timeSlots.length === 0 ? (
                        <p>No available time slots for this day.</p>
                    ) : (
                        <ul>
                            {timeSlots.map((slot) => (
                                <li key={slot.timeSlotId}>
                                    {slot.startTime} - {slot.endTime} ({slot.status})
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
            ) : (
                <p>Loading laundry room...</p>
            )}
        </div>
    );
};

export default Dashboard;

*/
 
