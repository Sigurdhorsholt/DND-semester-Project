// Setting.jsx
import React, { useEffect, useState } from "react";
import TimeSlotSettings from "./TimeSlotSettings";
import MachineSettings from "./MachineSettings";
import UserSettings from "./UserSettings";

export default function Settings({ settings, onSave }) {
  const [timeSlots, setTimeSlots] = useState([]);
  const [maxBookingsPerUser, setMaxBookingsPerUser] = useState(2); 
  const [laundryMachines, setLaundryMachines] = useState([]);
  const [allowShowUserInfo, setAllowShowUserInfo] = useState(false);

  useEffect(() => {

    console.log()
    


    if (settings) {

      console.log("setTimeSlots(settings.timeSlots || []); ", settings.timeSlots )
      console.log("setMaxBookingsPerUser(settings.maxBookingsPerUser || 2); ", settings.maxBookingsPerUser )
      console.log("setLaundryMachines(settings.laundryMachines || []); ", settings.laundryMachines)
      setTimeSlots(settings.timeSlots || []);
      setMaxBookingsPerUser(settings.maxBookingsPerUser || 2); 
      setLaundryMachines(settings.laundryMachines || []);
      setAllowShowUserInfo(settings.allowShowUserInfo || false);
    }
  }, [settings]);

  const handleSave = () => {
    const updatedSettings = {
      timeSlots,
      maxBookingsPerUser,
      laundryMachines,
      allowShowUserInfo,
    };
    onSave(updatedSettings);
  };

  return (
    <div className="p-4">
      <TimeSlotSettings timeSlots={timeSlots} setTimeSlots={setTimeSlots} />
      <MachineSettings
        laundryMachines={laundryMachines}
        setLaundryMachines={setLaundryMachines}
      />
      <UserSettings
        maxBooking={maxBookingsPerUser}
        setMaxBooking={setMaxBookingsPerUser}
        allowShowUserInfo={allowShowUserInfo} 
        setAllowShowUserInfo={setAllowShowUserInfo} 
      />
      <button
        onClick={handleSave}
        className="m-4 px-5 py-2 font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-green-600 rounded-lg hover:bg-green-500 focus:outline-none focus:ring focus:ring-green-300 focus:ring-opacity-80"
      >
        Gem Indstillinger
      </button>
    </div>
  );
}
