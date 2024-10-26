// MachineSettings.jsx
import React, { useState } from "react";

const MachineSettings = ({ laundryMachines, setLaundryMachines }) => {
  const [newMachine, setNewMachine] = useState({ name: "", type: "" });
  const [error, setError] = useState(null);

  const addLaundryMachine = () => {
    if (!newMachine.name || !newMachine.type) {
      setError("Machine name and type are required.");
      return;
    }
    setLaundryMachines([...laundryMachines, newMachine]);
    setNewMachine({ name: "", type: "" });
    setError(null);
  };

  return (
    <div>
      <h2>Laundry Machine Settings</h2>
      <input
        type="text"
        value={newMachine.name}
        placeholder="Machine Name"
        onChange={(e) =>
          setNewMachine({ ...newMachine, name: e.target.value })
        }
      />
      <select
        value={newMachine.type}
        onChange={(e) =>
          setNewMachine({ ...newMachine, type: e.target.value })
        }
      >
        <option value="">Select Machine Type</option>
        <option value="Washer">Washer</option>
        <option value="Dryer">Dryer</option>
      </select>
      <button onClick={addLaundryMachine}>Add Machine</button>
      {error && <p>{error}</p>}
      <ul>
        {laundryMachines.map((machine, index) => (
          <li key={index}>
            {machine.name} ({machine.type})
          </li>
        ))}
      </ul>
    </div>
  );
};

export default MachineSettings;
