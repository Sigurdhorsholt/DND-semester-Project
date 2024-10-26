// Login.js
import React, { useState, useContext } from "react";
import axios from "axios";
import { AuthContext } from "../../context/AuthContext";


const Login = () => {
    const { login } = useContext(AuthContext); // Use the login function from AuthContext
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            await login(username, password);

            /*
     const response = await axios.post("/api/auth/login", {
                username,
                password,
            });

            // Store the JWT token in localStorage
            localStorage.setItem("token", response.data.token);

            // Redirect to dashboard or protected route
            window.location.href = "/dashboard";

            */
        } catch (error) {
            setError("Invalid credentials. Please try again.");
        }
    };

    return (
        <div className="mt-8">
            <h2 className="text-2xl mb-4">Log In</h2>
            {error && <p className="text-red-500">{error}</p>}
            <form onSubmit={handleLogin} className="flex flex-col items-center">
                <div className="mb-4">
                    <label className="block text-sm font-medium">Username</label>
                    <input
                        type="text"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        className="px-4 py-2 rounded border"
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-sm font-medium">Password</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="px-4 py-2 rounded border"
                    />
                </div>
                <button
                    type="submit"
                    className="px-6 py-3 bg-white text-teal-500 font-semibold rounded-lg shadow-md hover:bg-gray-100"
                >
                    Log In
                </button>
            </form>
        </div>
    );
};

export default Login;
