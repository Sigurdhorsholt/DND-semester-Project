import React, { useEffect, useState } from "react";

export default function Setting({ settings, onSave }) {
  /*

Make it accept json obj that holds all settings from DB and populate with this
on load. update state on render based on these settings when implementing

*/

  const [timeSlots, setTimeSlots] = useState([]);
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [maxBooking, setMaxBooking] = useState(2);
  const [laundryMachines, setLaundryMachines] = useState([]);
  const [newMachine, setNewMachine] = useState({ name: "", type: "" });
  const [error, setError] = useState(null);

  useEffect(() => {
    if (settings) {
      setTimeSlots(settings.timeSlots || []);
      setMaxBooking(settings.maxBookings || 2);
      setLaundryMachines(settings.laundryMachines || []);
    }
  }, [settings]);

  // Add Laundry Machine
  const addLaundryMachine = () => {
    if (!newMachine.name || !newMachine.type) {
      setError("Machine name and type are required.");
      return;
    }
    setLaundryMachines([...laundryMachines, newMachine]);
    setNewMachine({ name: "", type: "" });
    setError(null);
  };

  // Handle Save
  const handleSave = () => {
    const updatedSettings = {
      timeSlots,
      maxBooking,
      laundryMachines,
    };
    onSave(updatedSettings); // Pass the updated settings back to the parent component
  };

  // Helper function to check if time slots overlap
  const checkOverlap = (newStart, newEnd) => {
    return timeSlots.some(
      (slot) =>
        (newStart >= slot.startTime && newStart < slot.endTime) ||
        (newEnd > slot.startTime && newEnd <= slot.endTime)
    );
  };

  // Add time slot
  const addTimeSlot = () => {
    if (!startTime || !endTime) {
      setError("Both start and end time are required.");
      return;
    }

    const newStartTime = new Date(`1970-01-01T${startTime}:00`);
    const newEndTime = new Date(`1970-01-01T${endTime}:00`);

    if (newEndTime <= newStartTime) {
      setError("End time must be after start time.");
      return;
    }

    if (checkOverlap(newStartTime, newEndTime)) {
      setError("Time slot overlaps with an existing slot.");
      return;
    }

    setTimeSlots([
      ...timeSlots,
      { startTime: newStartTime, endTime: newEndTime },
    ]);
    setStartTime("");
    setEndTime("");
    setError(null);
  };

  return (
    <div className="">
      <div className="mt-8 border-t-2 border-black-900">
        <h1 className="font-bold font-xl">Tids Indstillinger</h1>
        {/* Time Slot Section */}
        <div>
          <input
            type="time"
            value={startTime}
            onChange={(e) => setStartTime(e.target.value)}
            className="m-2 p-2 border rounded"
          />
          <input
            type="time"
            value={endTime}
            onChange={(e) => setEndTime(e.target.value)}
            className="m-2 p-2 border rounded"
          />
          <button
            onClick={addTimeSlot}
            className="m-4 px-5 py-2 font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-600 rounded-lg hover:bg-blue-500 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-80"
          >
            Tilføj
          </button>
          {error && <p className="text-red-500">{error}</p>}
        </div>
        <ul>
          {timeSlots.map((slot, index) => (
            <li key={index}>
              {slot.startTime.toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
              })}{" "}
              -{" "}
              {slot.endTime.toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </li>
          ))}
        </ul>
      </div>

      <div className="mt-8 border-t-2 border-black-900">
        <h1 className="font-bold font-xl">
          Vaske & Tørre Maskine Indstillinger
        </h1>
        {/* Laundry Machine Section */}
        <div className="flex flex-col">
          <input
            type="text"
            value={newMachine.name}
            placeholder="Navngiv Ny Maskine"
            onChange={(e) =>
              setNewMachine({ ...newMachine, name: e.target.value })
            }
            className="m-2 p-2 border rounded"
          />

          <select
            value={newMachine.type}
            onChange={(e) =>
              setNewMachine({ ...newMachine, type: e.target.value })
            }
            className="m-2 p-2 border rounded"
          >
            <option value="">Select Machine Type</option>
            <option value="Vaskemaskine">Vaskemaskine</option>
            <option value="Tørretumbler">Tørretumbler</option>
          </select>
          <button
            onClick={addLaundryMachine}
            className="inline-block m-4 px-5 py-2 font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-600 rounded-lg hover:bg-blue-500 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-80"
          >
            Tilføj Maskine
          </button>
          {error && <p className="text-red-500">{error}</p>}
        </div>
        <ol className="list-disc pl-4">
          {laundryMachines.map((machine, index) => (
            <li key={index}>
              {machine.name} ({machine.type})
            </li>
          ))}
        </ol>
      </div>

      <div className="mt-8 border-t-2 border-black-900">
        <h1 className="font-bold font-xl">Bruger Indstillinger</h1>
        <div className="flex items-center">
          <input
            type="number"
            value={maxBooking}
            onChange={(e) => {
              const value = parseInt(e.target.value, 10);
              if (Number.isInteger(value)) {
                setMaxBooking(value);
              }
            }}
            className="m-2 p-2 w-20 border rounded"
          />
          <p className="ml-2">Antal tilladte vasketider per bruger</p>
        </div>
      </div>
      <div className="">
        <div class="flex items-center mb-2">
          <input
            id="default-checkbox"
            type="checkbox"
            className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
          />
          <label htmlFor="default-checkbox" className="p-1">
            Tillad visning af brugeres navne
          </label>
        </div>

        <div class="flex items-center mb-2">
          <input
            id="default-checkbox"
            type="checkbox"
            className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600"
          />
          <label htmlFor="default-checkbox" className="p-1">
            Kan ikke finde på flere indstillinger
          </label>
        </div>
      </div>

      <button
        onClick={handleSave}
        className="m-4 px-5 py-2 font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-green-600 rounded-lg hover:bg-green-500 focus:outline-none focus:ring focus:ring-green-300 focus:ring-opacity-80"
      >
        Save Settings
      </button>
    </div>
  );
}
