import React, { useState, useEffect } from "react";

function Dateselector({ selectedDate, setSelectedDate }) {
  const [currentDate, setCurrentDate] = useState(new Date());

  // Function to get week dates
  const getWeekDates = (date) => {
    const week = [];
    const current = new Date(date);
    const firstDayOfWeek = current.getDate() - current.getDay() + 1;
    
    for (let i = 0; i < 7; i++) {
      const day = new Date(current);
      day.setDate(firstDayOfWeek + i);
      week.push(day);
    }
    
    return week;
  };

  const [weekDates, setWeekDates] = useState(getWeekDates(currentDate));

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
    <div className="Dateselector">
      {/* Week Navigation */}
      <div className="flex justify-between bg-gray-100 p-4">
        <button onClick={goToPreviousWeek} className="bg-teal-500 text-white px-4 py-2 rounded">
          Previous
        </button>
        <button onClick={goToNextWeek} className="bg-teal-500 text-white px-4 py-2 rounded">
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
              ? "bg-teal-500 text-white rounded-full "
              : ""
            }`}
            onClick={() => setSelectedDate(date)}  
          >
            <div>{date.toLocaleString("default", { weekday: "short" })}</div>
            <div>{date.getDate() +"/"+ (date.getMonth()+1)}</div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default Dateselector;
