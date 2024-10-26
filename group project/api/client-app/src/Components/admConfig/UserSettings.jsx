// UserSettings.jsx
import React from "react";

const UserSettings = ({ maxBooking, setMaxBooking }) => {
  return (
    <div>
      <h2>User Settings</h2>
      <input
        type="number"
        value={maxBooking}
        onChange={(e) => setMaxBooking(parseInt(e.target.value, 10))}
      />
      <p>Max bookings per user</p>
    </div>
  );
};

export default UserSettings;
