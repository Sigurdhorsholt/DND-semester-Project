// src/components/ProtectedRoute.jsx
import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from "../../context/AuthContext";
import { jwtDecode } from 'jwt-decode';

const ProtectedRoute = ({ children, adminOnly }) => {
    const { user, loading } = useContext(AuthContext);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (!user) {
        return <Navigate to="/login" />;
    }

    // Check if the user is an admin if the route requires admin access
    if (adminOnly) {
        const decodedToken = jwtDecode(user.token);
        const isAdmin = decodedToken.IsAdmin === "true";
        if (!isAdmin) {
            return <Navigate to="/user-dashboard" />; // Redirect non-admin users to the user dashboard
        }
    }

    return children;
};

export default ProtectedRoute;
