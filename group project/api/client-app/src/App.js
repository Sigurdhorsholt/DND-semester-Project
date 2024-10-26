import React from 'react';
import {BrowserRouter as Router, Link, Route, Routes} from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Home from './pages/Home';
import Login from "./components/Auth/Login.jsx";
import UserDashboard from './Pages/Users/UserDashboard';
import AdminDashboard from './Pages/Adm/AdminDashboard';
import './App.css';
import ProtectedRoute from "./Components/Auth/ProtectedRoute";
import Navbar from './Components/Navbar';

function App() {
  return (
          <Router>
              <AuthProvider>

              <div>
                 <Navbar/>
                 
              <Routes> 
                  <Route path="/" element={<Home />} /> 
                  <Route path="/login" element={<Login />} />
                  <Route path="/Dashboard" element={<UserDashboard />} />
                  <Route
                      path="/user-dashboard"
                      element={
                          <ProtectedRoute>
                              <UserDashboard />
                          </ProtectedRoute>
                      }
                  />
                  <Route
                      path="/admin-dashboard"
                      element={
                          <ProtectedRoute adminOnly={true}>
                              <AdminDashboard />
                          </ProtectedRoute>
                      }
                  />
                  
                  {/*<Route path="/user-dashboard" element={<UserDashboard />} />
                  <Route path="/admin-dashboard" element={<AdminDashboard />} />*/}
              </Routes>
              </div>
              </AuthProvider>

          </Router>
  );
}

export default App;