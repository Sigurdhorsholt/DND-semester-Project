// src/components/Navbar.jsx
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

const Navbar = () => {
  const { user, logout, getDecodedToken } = useContext(AuthContext); // Get the user, logout, and getDecodedToken from AuthContext

  const decodedToken = getDecodedToken();
  const isAdmin = decodedToken?.IsAdmin === "true"; // Check if IsAdmin is "true" in the decoded token

  console.log("from navbar: " + isAdmin);

  return (
    <>
      <nav className="p-4 bg-gray-100">
        <ul className="flex space-x-4">
          <li>
            <Link to="/" className="text-blue-500">
              Home
            </Link>
          </li>
          {user ? (
            <>
              {isAdmin && (
                <>
           
                  <li>
                    <Link to="/admin-dashboard" className="text-blue-500">
                      Admin Dashboard
                    </Link>
                  </li>
                </>
              )}
              <li>
                <Link to="/user-dashboard" className="text-blue-500">
                  User Dashboard
                </Link>
              </li>
              <li>
                <Link to="/user-dashboard" className="text-blue-500">
                  Profile
                </Link>
              </li>
              <li>
                <button onClick={logout} className="text-red-500">
                  Log Out
                </button>
              </li>
            </>
          ) : (
            <li>
              <Link to="/login" className="text-blue-500">
                Log In
              </Link>
            </li>
          )}
        </ul>
      </nav>
    </>
  );
};

export default Navbar;
