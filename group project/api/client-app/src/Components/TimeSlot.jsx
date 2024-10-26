import React from 'react';

function TimeSlot({ apartment, startTime, endTime, isAvailable, selectedDate, onBookSlot }) {

  const date = new Date(selectedDate);
  const dayOfWeek = date.toLocaleString('default', { weekday: 'long' });
  const formattedDate = date.toLocaleDateString();

  const cardClicked = () => {
    if (isAvailable) {
      onBookSlot(apartment, startTime, endTime); // Trigger booking
    } else {
      console.log("Slot not available");
    }
  };

  return (
    <div className="pt-4">
      <div 
        onClick={cardClicked}
        className={`pt-4 rounded-md shadow-md w-3/5 mx-auto ${isAvailable ? 'bg-green-500' : 'bg-red-500'} text-white transform transition-transform duration-200 hover:scale-105`}
      >
        <div className="flex-grow text-center">
          <h3 className="text-2xl font-semibold">{`${startTime} - ${endTime}`}</h3>
          <p className="text-sm">{`${dayOfWeek}, ${formattedDate}`}</p>
        </div>

        <div className="flex justify-end">
          {!isAvailable && <p className="ml-4 text-xl font-medium">{apartment}</p>}
        </div>
      </div>
    </div>
  );
}

export default TimeSlot;
