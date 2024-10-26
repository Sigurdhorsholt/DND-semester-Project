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
