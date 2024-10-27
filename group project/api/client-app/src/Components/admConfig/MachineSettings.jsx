import React, { useState, useEffect } from "react";

const MachineSettings = ({ laundryMachines, setLaundryMachines }) => {
  const [newMachine, setNewMachine] = useState({ name: "", type: "" });
  const [error, setError] = useState(null);

  const addLaundryMachine = () => {
    if (!newMachine.name || !newMachine.type) {
      setError("Maskinens navn og type er påkrævet."); // Translated: Machine name and type are required.
      return;
    }
    setLaundryMachines([...laundryMachines, newMachine]);
    setNewMachine({ name: "", type: "" });
    setError(null);
  };

  return (
    <div className="mt-8 border-t-2 border-black-900">
      <h2 className="font-bold font-xl">Vaske & Tørre Maskine Indstillinger</h2> {/* Translated: Laundry Machine Settings */}
      <input
        type="text"
        value={newMachine.name}
        placeholder="Navngiv Ny Maskine" // Translated: Name New Machine
        onChange={(e) => setNewMachine({ ...newMachine, name: e.target.value })}
        className="m-2 p-2 border rounded"
      />
      <select
        value={newMachine.type}
        onChange={(e) => setNewMachine({ ...newMachine, type: e.target.value })}
        className="m-2 p-2 border rounded"
      >
        <option value="">Vælg Maskine Type</option> {/* Translated: Select Machine Type */}
        <option value="Vaskemaskine">Vaskemaskine</option> {/* Translated: Washer */}
        <option value="Tørretumbler">Tørretumbler</option> {/* Translated: Dryer */}
      </select>
      <button
        onClick={addLaundryMachine}
        className="inline-block m-4 px-5 py-2 font-medium tracking-wide text-white capitalize transition-colors duration-300 transform bg-blue-600 rounded-lg hover:bg-blue-500 focus:outline-none focus:ring focus:ring-blue-300 focus:ring-opacity-80"
      >
        Tilføj Maskine {/* Translated: Add Machine */}
      </button>
      {error && <p className="text-red-500">{error}</p>}
      <ul>
        {laundryMachines.map((machine, index) => (
          <li key={index}>
            {machine.machineName} ({machine.machineType})
          </li>
        ))}
      </ul>
    </div>
  );
};

export default MachineSettings;
