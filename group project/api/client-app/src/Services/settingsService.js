// src/services/settingsService.js
import axios from "axios";

const getSettings = async (roomId) => {
  try {
    const response = await axios.get(`/api/laundry-room/${roomId}/settings`);
    return response.data;
  } catch (error) {
    console.error("Failed to fetch settings:", error);
    throw error;
  }
};

const saveSettings = async (roomId, settings) => {
  try {
    const response = await axios.post(`/api/laundry-room/${roomId}/settings`, settings);
    return response.data;
  } catch (error) {
    console.error("Failed to save settings:", error);
    throw error;
  }
};

// Export an object with both functions as a default export
export default { getSettings, saveSettings };
