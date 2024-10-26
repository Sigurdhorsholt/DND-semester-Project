// src/context/AuthContext.jsx
import React, {createContext, useEffect, useState} from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';



export const AuthContext = createContext();

// Function to get the decoded token



export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    
    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
            
            setUser({token});
        }
        setLoading(false);
    }, []);

     const getDecodedToken = () => {
        const token = localStorage.getItem("token");
        if (token) {
            try {
                return jwtDecode(token);
            } catch (error) {
                console.log("Failed to decode token:", error);
            }
        }
        return null; // Return null if there is no valid token
    };
    
    const login = async (username, password) => {
        try {
            const response = await axios.post("/api/auth/login", {
                username,
                password,
            });
            
            const token = response.data.token;
            
            console.log("response "+response)
            console.log("response.data "+response.data)
            console.log("response.data.token "+response.data.token)

            console.log("token: "+ token)



            if (!token || typeof token !== "string") {
                throw new Error("Invalid token format");
            }


            localStorage.setItem("token", token);
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + token;
            setUser({token});

            // Decode the token to get user claims
            const decodedToken = jwtDecode(token);
            
            console.log("token: " + decodedToken)

            const isAdmin = decodedToken.IsAdmin === "true";
            
            console.log("token: " + isAdmin)

            

            if (isAdmin) {
                navigate("/admin-dashboard");
            } else
            {
                navigate("/user-dashboard");
            }
            
        }catch(error) {
            console.log("Login Failed: ", error);
        }
        
    }
    // Function to log out
    const logout = () => {
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
        setUser(null);
        navigate('/login'); // Redirect to login page after logout
    };



    
    
    
    
    return (
        <AuthContext.Provider value={{ user, login, logout, loading, getDecodedToken  }}>
            {children}
        </AuthContext.Provider>
    );
};
