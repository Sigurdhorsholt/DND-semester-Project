// Home.js
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import Login from "../components/Auth/Login.jsx"; // Import the Login component
import { AuthContext } from "../context/AuthContext";

const Home = () => {
  const { user, logout } = useContext(AuthContext); // Get the user and logout from AuthContext


  return (
    <div className="min-h-screen flex flex-col">
      {/* Header Section */}
      <header className="bg-teal-500 text-white py-12 px-4 text-center">
        {user ? (
          <>
            <h1 className="text-4xl font-bold">Welcome, {user.token}</h1>
            <button
              onClick={logout}
              className="mt-4 px-6 py-3 bg-red-500 text-white font-semibold rounded-lg shadow-md hover:bg-red-600"
            >
              Log Out
            </button>
          </>
        ) : (
          <>
            <h1 className="text-4xl font-bold">Welcome to the Application</h1>
            <Login />
            <div className="p-5 m-5">
              <Link
                to="/signup"
                className="px-6 py-3 bg-teal-700 text-white font-semibold rounded-lg shadow-md hover:bg-teal-600"
              >
                Sign Up
              </Link>
            </div>
          </>
        )}
      </header>

      {/* Footer Section */}
      <footer className="bg-gray-800 text-white py-6 text-center mt-auto">
        <p>Footer Content Here</p>
      </footer>
    </div>
  );
};

export default Home;
