// Setting.jsx
import React, { useEffect, useState } from "react";
import TimeSlotSettings from "./TimeSlotSettings";
import MachineSettings from "./MachineSettings";
import UserSettings from "./UserSettings";




export default function Settings({ settings, onSave }) {
  const [timeSlots, setTimeSlots] = useState([]);
  const [maxBooking, setMaxBooking] = useState(2);
  const [laundryMachines, setLaundryMachines] = useState([]);

  useEffect(() => {
    if (settings) {
      setTimeSlots(settings.timeSlots || []);
      setMaxBooking(settings.maxBookings || 2);
      setLaundryMachines(settings.laundryMachines || []);
    }
  }, [settings]);

  const handleSave = () => {
    const updatedSettings = {
      timeSlots,
      maxBooking,
      laundryMachines,
    };
    onSave(updatedSettings);
  };

  return (
    <div>
      <TimeSlotSettings timeSlots={timeSlots} setTimeSlots={setTimeSlots} />
      <MachineSettings laundryMachines={laundryMachines} setLaundryMachines={setLaundryMachines} />
      <UserSettings maxBooking={maxBooking} setMaxBooking={setMaxBooking} />
      <button onClick={handleSave} className="m-4 px-5 py-2 bg-green-600 text-white rounded">
        Save Settings
      </button>
    </div>
  );
}
