import React, { useState, useContext } from "react";
import { Link, NavLink } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";
import { FaBars, FaTimes } from "react-icons/fa"; // Icons for burger menu

const Navbar = () => {
  const { user, logout, getDecodedToken } = useContext(AuthContext);
  const [isMenuOpen, setIsMenuOpen] = useState(false); // State for the burger menu

  const decodedToken = getDecodedToken();
  const isAdmin = decodedToken?.IsAdmin === "true"; // Check if user is admin

  // Toggle menu visibility
  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  return (
    <nav className="bg-gray-100 shadow-md">
      <div className="container mx-auto flex items-center justify-between p-4">
        {/* Logo */}
        <div className="text-2xl font-bold">
          <Link to="/" className="text-blue-500">
            Laundry Booker
          </Link>
        </div>

        {/* Burger Menu Icon for small screens */}
        <div className="md:hidden">
          <button onClick={toggleMenu} aria-label="Toggle Menu">
            {isMenuOpen ? <FaTimes size={24} /> : <FaBars size={24} />}
          </button>
        </div>

        {/* Navigation Links */}
        <ul
          className={`${
            isMenuOpen ? "block" : "hidden"
          } md:flex md:space-x-6 absolute md:static top-full right-0 w-full md:w-auto bg-gray-100 md:bg-transparent p-4 md:p-0 z-50`}
        >
          {/* Home */}
          <li>
            <NavLink
              to="/"
              exact
              className={({ isActive }) =>
                isActive ? "text-red-500" : "text-blue-500"
              }
            >
              Home
            </NavLink>
          </li>

          {/* Conditionally show admin and user dashboard links */}
          {user && (
            <>
              {isAdmin && (
                <li>
                  <NavLink
                    to="/admin-dashboard"
                    className={({ isActive }) =>
                      isActive ? "text-red-500" : "text-blue-500"
                    }
                  >
                    Admin Dashboard
                  </NavLink>
                </li>
              )}
              <li>
                <NavLink
                  to="/user-dashboard"
                  className={({ isActive }) =>
                    isActive ? "text-red-500" : "text-blue-500"
                  }
                >
                  User Dashboard
                </NavLink>
              </li>
              <li>
                <NavLink
                  to="/profile"
                  className={({ isActive }) =>
                    isActive ? "text-red-500" : "text-blue-500"
                  }
                >
                  Profile
                </NavLink>
              </li>
              <li>
                <button
                  onClick={logout}
                  className="text-red-500 focus:outline-none"
                >
                  Log Out
                </button>
              </li>
            </>
          )}

          {/* Show login if no user is logged in */}
          {!user && (
            <li>
              <NavLink
                to="/login"
                className={({ isActive }) =>
                  isActive ? "text-red-500" : "text-blue-500"
                }
              >
                Log In
              </NavLink>
            </li>
          )}
        </ul>
      </div>
    </nav>
  );
};

export default Navbar;
