import React, { useState, useEffect } from "react";
import Settings from "../../Components/admConfig/Setting_Old";
import SettingsService from "../../Services/settingsService";

function AdminDashboard() {
  const [selectedRoom, setSelectedRoom] = useState(null);
  const [view, setView] = useState("admin");

  const [currentSettings, setCurrentSettings] = useState(null);

  useEffect(() => {
    if (selectedRoom) {
      loadSettings(selectedRoom.id);
    }
  }, [selectedRoom]);

  const loadSettings = async (roomId) => {
    try {
      const settings = await SettingsService.getSettings(roomId);
      setCurrentSettings(settings);
    } catch (error) {
      console.error("Failed to load settings:", error);
    }
  };

  const handleSave = async (settings) => {
    try {
      await SettingsService.saveSettings(selectedRoom.id, settings);
      alert("Settings saved successfully!");
    } catch (error) {
      alert("Failed to save settings.");
    }
  };

  //placeholder data to be passed on constructor
  const laundryRooms = [
    {
      id: 12343,
      name: "Vestre Ringgade 218",
      address: "Vestre Ringgade 218, 8000 Aarhus, Danmark",
    },
    {
      id: 24324,
      name: "TelefonTorvet 2B",
      address: "TelefonTorvet 2B, 8000 Aarhus, Danmark",
    },
    {
      id: 34323,
      name: "Hasle Ringgade",
      address: "Hasle Ringgade 47, 8000 Aarhus, Danmark",
    },
  ];

  const users = [
    {
      id: 1234,
      apartment: "4TH",
      lastLoggedin: "12/09",
    },
    {
      id: 1234,
      apartment: "2TV",
      lastLoggedin: "09/09",
    },
    {
      id: 1234,
      apartment: "1ST",
      lastLoggedin: "08/07",
    },
  ];

  const handleRoomSelect = (room) => {
    setSelectedRoom(room);
    setView("admin");
  };

  return (
    <>
      <div className="flex h-screen">
        {/* Left Panel - Room List */}
        <div className="w-1/4 bg-gray-100 p-4">
          <h2 className="text-xl font-bold mb-4">Laundry Rooms</h2>
          <ul>
            {laundryRooms.map((room) => (
              <li
                key={room.id}
                className={`p-2 cursor-pointer ${
                  selectedRoom?.id === room.id ? "bg-teal-500 text-white" : ""
                }`}
                onClick={() => handleRoomSelect(room)}
              >
                <div>{room.name}</div>
                <div>{"Id: " + room.id}</div>
              </li>
            ))}
          </ul>
        </div>

        {/* Middle Panel - Users or Admin Config */}
        <div className="w-3/4 p-4">
          {selectedRoom ? (
            <>
              <p>NEW SETUP</p>
              <div>
                <Settings
                  settings={currentSettings}
                  selectedRoom={selectedRoom}
                  onSave={handleSave}
                />
              </div>

              <p>OLD SETUP</p>

              <div className="flex justify-between items-center mb-4">
                <h2 className="text-xl font-bold">{selectedRoom.name}</h2>
                <div>
                  <button
                    className={`px-4 py-2 mr-2 ${
                      view === "users"
                        ? "bg-teal-500 text-white"
                        : "bg-gray-200"
                    }`}
                    onClick={() => setView("users")}
                  >
                    Users
                  </button>

                  <button
                    className={`px-4 py-2 ${
                      view === "config"
                        ? "bg-teal-500 text-white"
                        : "bg-gray-200"
                    }`}
                    onClick={() => setView("config")}
                  >
                    Admin Config
                  </button>
                </div>
              </div>

              {/* Toggleable Views */}
              {view === "users" ? (
                <div>
                  <h3 className="text-lg font-bold mb-2">
                    Users in {selectedRoom.name}
                  </h3>
                  {/* Here you can list users, fetched from an API */}
                  <ul>
                    {users.map((user) => (
                      <li
                        key={user.id}
                        className="pt-4 
                            transform transition-transform duration-100 hover:scale-"
                      >
                        Apartment: {user.apartment}, Last Logged in:{" "}
                        {user.lastLoggedin}
                      </li>
                    ))}
                  </ul>
                </div>
              ) : (
                <div>
                  <h3 className="text-lg font-bold mb-2">
                    Admin Config for {selectedRoom.address}
                  </h3>
                  {/* Here you can display admin config options */}

                  <Settings />
                </div>
              )}
            </>
          ) : (
            <p>Please select a laundry room to manage.</p>
          )}
        </div>
      </div>
    </>
  );
}

export default AdminDashboard;
