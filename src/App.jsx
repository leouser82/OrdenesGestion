import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Navbar from './components/Navbar';
import Login from './components/Login';
import OrdenesPage from './pages/OrdenesPage';
import NuevaOrdenPage from './pages/NuevaOrdenPage';

function App() {
  return (
    <AuthProvider>
      <div className="App">
        <Navbar />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route 
            path="/ordenes" 
            element={
              <ProtectedRoute>
                <OrdenesPage />
              </ProtectedRoute>
            } 
          />
          <Route 
            path="/nueva-orden" 
            element={
              <ProtectedRoute>
                <NuevaOrdenPage />
              </ProtectedRoute>
            } 
          />
          <Route path="/" element={<Navigate to="/ordenes" replace />} />
          <Route path="*" element={<Navigate to="/ordenes" replace />} />
        </Routes>
      </div>
    </AuthProvider>
  );
}

export default App;
