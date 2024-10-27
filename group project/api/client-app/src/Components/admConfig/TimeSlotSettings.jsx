import React, { useState, useEffect } from "react";

const TimeSlotSettings = ({ timeSlots, setTimeSlots }) => {
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [error, setError] = useState(null);

  // Helper function to check if time slots overlap
  const checkOverlap = (newStart, newEnd) => {
    return timeSlots.some(
      (slot) =>
        (newStart >= new Date(`1970-01-01T${slot.startTime}`) && newStart < new Date(`1970-01-01T${slot.endTime}`)) ||
        (newEnd > new Date(`1970-01-01T${slot.startTime}`) && newEnd <= new Date(`1970-01-01T${slot.endTime}`))
    );
  };

  // Add time slot
  const addTimeSlot = () => {
    if (!startTime || !endTime) {
      setError("Både start- og sluttid er påkrævet."); // Translated: Both start and end time are required.
      return;
    }

    const newStartTime = new Date(`1970-01-01T${startTime}:00`);
    const newEndTime = new Date(`1970-01-01T${endTime}:00`);

    if (newEndTime <= newStartTime) {
      setError("Sluttid skal være efter starttid."); // Translated: End time must be after start time.
      return;
    }

    if (checkOverlap(newStartTime, newEndTime)) {
      setError("Tidsinterval overlapper med et eksisterende interval."); // Translated: Time slot overlaps with an existing slot.
      return;
    }

    setTimeSlots([...timeSlots, { startTime: startTime, endTime: endTime }]);
    setStartTime("");
    setEndTime("");
    setError(null);
  };

  return (
    <div className="mt-8 border-t-2 border-black-900">
      <h2 className="font-bold font-xl">Tids Indstillinger</h2> {/* Translated: Time Slot Settings */}
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
        Tilføj {/* Translated: Add */}
      </button>
      {error && <p className="text-red-500">{error}</p>}
      <ul>
        {timeSlots.map((slot, index) => (
          <li key={index}>
            {slot.startTime} - {slot.endTime}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TimeSlotSettings;
