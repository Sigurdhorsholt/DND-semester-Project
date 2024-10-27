import React, { useState, useEffect } from "react";
import Settings from "../../Components/admConfig/Settings";
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

console.log("Passed settings Settings:", settings)

const formattedTimeSlots = settings.timeSlots.map(slot => ({
  startTime: new Date(slot.startTime).toLocaleTimeString('en-GB', { hour12: false }),
  endTime: new Date(slot.endTime).toLocaleTimeString('en-GB', { hour12: false })
}));


const formattedSettings = {
  maxBookingsPerUser: settings.maxBooking,  
  timeSlots: formattedTimeSlots,            
  laundryMachines: settings.laundryMachines.map(machine => ({
    machineName: machine.name,
    machineType: machine.type
  })),
  allowShowUserInfo: settings.allowShowUserInfo 
};

console.log("Formatted Settings:", formattedSettings); // For debugging



    try {
      await SettingsService.saveSettings(selectedRoom.id, formattedSettings);
      alert("Settings saved successfully!");
    } catch (error) {
      alert("Failed to save settings.");
    }
  };

  //placeholder data to be passed on constructor
  const laundryRooms = [
    {
      id: 1,
      name: "Vestre Ringgade 218",
      address: "Vestre Ringgade 218, 8000 Aarhus, Danmark",
    },
    {
      id: 2,
      name: "TelefonTorvet 2B",
      address: "TelefonTorvet 2B, 8000 Aarhus, Danmark",
    },
    {
      id: 3,
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

              <div>
                <Settings
                  settings={currentSettings}
                  selectedRoom={selectedRoom}
                  onSave={handleSave}
                />
              </div>


              
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
