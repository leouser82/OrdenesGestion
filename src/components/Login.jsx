import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { LogIn, User, Lock, AlertCircle } from 'lucide-react';

const Login = () => {
  const [credentials, setCredentials] = useState({
    username: '',
    password: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e) => {
    setCredentials({
      ...credentials,
      [e.target.name]: e.target.value
    });
    // Limpiar error cuando el usuario empiece a escribir
    if (error) setError('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await login(credentials);
      navigate('/ordenes');
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fillDemoCredentials = (role) => {
    if (role === 'admin') {
      setCredentials({ username: 'admin', password: 'admin123' });
    } else {
      setCredentials({ username: 'user', password: 'user123' });
    }
  };

  return (
    <div className="container" style={{ maxWidth: '400px', marginTop: '80px' }}>
      <div className="card">
        <div style={{ textAlign: 'center', marginBottom: '32px' }}>
          <LogIn size={48} style={{ color: '#667eea', marginBottom: '16px' }} />
          <h1 style={{ color: '#374151', marginBottom: '8px' }}>
            Iniciar Sesión
          </h1>
          <p style={{ color: '#6b7280' }}>
            Sistema de Gestión de Órdenes de Inversión
          </p>
        </div>

        {error && (
          <div className="alert alert-error">
            <AlertCircle size={16} style={{ marginRight: '8px' }} />
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">
              <User size={16} style={{ marginRight: '8px', display: 'inline' }} />
              Usuario
            </label>
            <input
              type="text"
              id="username"
              name="username"
              value={credentials.username}
              onChange={handleChange}
              className="form-control"
              placeholder="Ingresa tu usuario"
              required
              disabled={loading}
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">
              <Lock size={16} style={{ marginRight: '8px', display: 'inline' }} />
              Contraseña
            </label>
            <input
              type="password"
              id="password"
              name="password"
              value={credentials.password}
              onChange={handleChange}
              className="form-control"
              placeholder="Ingresa tu contraseña"
              required
              disabled={loading}
            />
          </div>

          <button 
            type="submit" 
            className="btn btn-primary" 
            style={{ width: '100%', marginBottom: '16px' }}
            disabled={loading}
          >
            {loading ? (
              <div className="spinner" style={{ width: '20px', height: '20px' }}></div>
            ) : (
              'Iniciar Sesión'
            )}
          </button>
        </form>

        <div style={{ borderTop: '1px solid #e5e7eb', paddingTop: '16px', marginTop: '16px' }}>
          <p style={{ color: '#6b7280', fontSize: '14px', marginBottom: '12px' }}>
            Credenciales de prueba:
          </p>
          <div style={{ display: 'flex', gap: '8px', flexWrap: 'wrap' }}>
            <button
              type="button"
              onClick={() => fillDemoCredentials('admin')}
              className="btn btn-secondary"
              style={{ fontSize: '12px', padding: '6px 12px' }}
              disabled={loading}
            >
              Admin
            </button>
            <button
              type="button"
              onClick={() => fillDemoCredentials('user')}
              className="btn btn-secondary"
              style={{ fontSize: '12px', padding: '6px 12px' }}
              disabled={loading}
            >
              Usuario
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
