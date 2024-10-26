import React, { useState } from "react";
import "./App.css";

function App() {





  
  const [currentDate, setCurrentDate] = useState(new Date());
  const [selectedDate, setSelectedDate] = useState(null);

// Function to get week dates
const getWeekDates = (date) => {
  const week = [];
  const current = new Date(date);
  
  // Calculate the first day of the week (Monday)
  const firstDayOfWeek = current.getDate() - current.getDay() + 1; // Assuming Sunday is 0
  
  // Loop to create new Date objects for each day of the week
  for (let i = 0; i < 7; i++) {
    // Create a new Date instance for each day to avoid modifying the same object
    const day = new Date(current);
    day.setDate(firstDayOfWeek + i);
    week.push(day); // Push the cloned day
  }
  
  return week;
};

  const [weekDates, setWeekDates] = useState(getWeekDates(currentDate));

  const handleDateClick = (date) => {
    console.log(`Date clicked: ${date.toDateString()}`);
    setSelectedDate(date);
  };

  const goToPreviousWeek = () => {
    const previousWeek = new Date(currentDate.setDate(currentDate.getDate() - 7));
    setCurrentDate(previousWeek);
    setWeekDates(getWeekDates(previousWeek));
  };

  const goToNextWeek = () => {
    const nextWeek = new Date(currentDate.setDate(currentDate.getDate() + 7));
    setCurrentDate(nextWeek);
    setWeekDates(getWeekDates(nextWeek));
  };

  const isToday = (date) => {
    const today = new Date();
    return (
      date.getDate() === today.getDate() &&
      date.getMonth() === today.getMonth() &&
      date.getFullYear() === today.getFullYear()
    );
  };

  return (
    <div className="App">


      {/* Header */}
      <header className="bg-teal-500 text-white p-4 flex justify-between items-center sticky top-0">
        <div className="text-xl font-bold">VASKERUM.DK</div>
        <div className="text-2xl">
          <i className="fas fa-bars"></i>
        </div>
      </header>

      {/* Week Navigation */}
      <div className="flex justify-between bg-gray-100 p-4">
        <button
          onClick={goToPreviousWeek}
          className="bg-teal-500 text-white px-4 py-2 rounded"
        >
          Previous
        </button>
        <button
          onClick={goToNextWeek}
          className="bg-teal-500 text-white px-4 py-2 rounded"
        >
          Next
        </button>
      </div>

      {/* Date Selector */}
      <div className="flex justify-around bg-gray-100 p-4">
        {weekDates.map((date, index) => (
          <div
            key={index}
            className={`cursor-pointer text-center p-2 ${
              isToday(date) ? "text-teal-500 font-bold" : ""
            } ${selectedDate && selectedDate.toDateString() === date.toDateString()
              ? "bg-teal-500 text-white rounded-full"
              : ""
            }`}
            onClick={() => handleDateClick(date)}
          >
            <div>{date.toLocaleString("default", { weekday: "short" })}</div>
            <div>{date.getDate() +"/"+ (date.getMonth()+1)}</div>
          </div>
        ))}
      </div>

      {/* Tabs */}
      <div className="flex justify-around my-4">
        <button className="flex-1 py-2 bg-teal-500 text-white">Vaske</button>
        <button className="flex-1 py-2 bg-gray-100">Tørre</button>
      </div>

      {/* Booking List */}
      <div className="space-y-4 my-6">
        <div className="bg-gray-200 p-4 rounded-md shadow-md w-4/5 mx-auto">
          Mark - 9:00 to 12:00
        </div>
        <div className="bg-gray-200 p-4 rounded-md shadow-md w-4/5 mx-auto">
          Min Booking - 13:00 to 16:00
        </div>
        <div className="bg-gray-200 p-4 rounded-md shadow-md w-4/5 mx-auto">
          Johanne - 16:00 to 19:00
        </div>
      </div>

      {/* Add Button */}
      <button className="bg-teal-500 text-white rounded-full w-16 h-16 text-3xl fixed bottom-6 right-6 flex justify-center items-center shadow-lg">
        <i className="fas fa-plus"></i>
      </button>
    </div>
  );
}

export default App;
