import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { TrendingUp, LogOut, User, Plus, List } from 'lucide-react';

const Navbar = () => {
  const { user, logout, isAdmin } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!user) return null;

  return (
    <nav className="navbar">
      <div className="container">
        <div className="navbar-content">
          <Link to="/ordenes" className="navbar-brand">
            <TrendingUp size={24} style={{ marginRight: '8px', display: 'inline' }} />
            Gestión de Órdenes
          </Link>

          <div className="navbar-nav">
            <Link to="/ordenes" className="nav-link">
              <List size={16} style={{ marginRight: '4px' }} />
              Órdenes
            </Link>
            
            <Link to="/nueva-orden" className="nav-link">
              <Plus size={16} style={{ marginRight: '4px' }} />
              Nueva Orden
            </Link>

            <div style={{ 
              display: 'flex', 
              alignItems: 'center', 
              gap: '12px',
              marginLeft: '16px',
              paddingLeft: '16px',
              borderLeft: '1px solid #e5e7eb'
            }}>
              <div style={{ 
                display: 'flex', 
                alignItems: 'center', 
                gap: '6px',
                color: '#374151',
                fontSize: '14px'
              }}>
                <User size={16} />
                <span>{user.username}</span>
                {isAdmin() && (
                  <span style={{ 
                    background: '#667eea',
                    color: 'white',
                    fontSize: '10px',
                    padding: '2px 6px',
                    borderRadius: '4px',
                    fontWeight: '500'
                  }}>
                    ADMIN
                  </span>
                )}
              </div>
              
              <button
                onClick={handleLogout}
                className="btn btn-secondary"
                style={{ 
                  padding: '6px 12px',
                  fontSize: '14px'
                }}
              >
                <LogOut size={16} />
                Salir
              </button>
            </div>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
