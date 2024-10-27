import React from "react";

const UserSettings = ({ maxBookingsPerUser, setMaxBookingsPerUser, allowShowUserInfo, setAllowShowUserInfo }) => {
  return (
    <div className="mt-8 border-t-2 border-black-900">
      <h2 className="font-bold font-xl">Bruger Indstillinger</h2> {/* Translated: User Settings */}
      <div className="flex items-center">
        <input
          type="number"
          value={maxBookingsPerUser} // Use maxBookingsPerUser
          onChange={(e) => setMaxBookingsPerUser(parseInt(e.target.value, 10))} // Update setter
          className="m-2 p-2 w-20 border rounded"
        />
        <p className="ml-2">Antal tilladte vasketider per bruger</p> {/* Translated: Max bookings per user */}
      </div>

      <div className="mt-4">
        <div className="flex items-center mb-2">
          <input
            id="allow-show-user-info-checkbox"
            type="checkbox"
            checked={allowShowUserInfo} // Bind the checkbox to state
            onChange={() => setAllowShowUserInfo(!allowShowUserInfo)} // Toggle state when clicked
            className="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded focus:ring-blue-500"
          />
          <label htmlFor="allow-show-user-info-checkbox" className="ml-2">
            Tillad visning af brugeres navne {/* Translated: Allow showing user info */}
          </label>
        </div>
      </div>
    </div>
  );
};

export default UserSettings;
