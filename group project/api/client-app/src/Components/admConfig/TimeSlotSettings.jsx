// TimeSlotSettings.jsx
import React, { useState } from "react";

const TimeSlotSettings = ({ timeSlots, setTimeSlots }) => {
  const [startTime, setStartTime] = useState("");
  const [endTime, setEndTime] = useState("");
  const [error, setError] = useState(null);

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
    <div>
      <h2>Time Slot Settings</h2>
      <input
        type="time"
        value={startTime}
        onChange={(e) => setStartTime(e.target.value)}
      />
      <input
        type="time"
        value={endTime}
        onChange={(e) => setEndTime(e.target.value)}
      />
      <button onClick={addTimeSlot}>Add Time Slot</button>
      {error && <p>{error}</p>}
      <ul>
        {timeSlots.map((slot, index) => (
          <li key={index}>
            {slot.startTime.toLocaleTimeString()} - {slot.endTime.toLocaleTimeString()}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TimeSlotSettings;
