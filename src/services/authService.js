import axios from 'axios';

const API_BASE_URL = 'http://localhost:7051/api';

// Configurar axios con interceptor para incluir token automáticamente
const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 segundos timeout
  withCredentials: false // Cambiar a false para evitar problemas de CORS
});

// Interceptor para agregar token a las peticiones
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor para manejar respuestas y errores de autenticación
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authService = {
  // Login del usuario
  async login(credentials) {
    try {
      const response = await apiClient.post('/auth/login', credentials);
      const { token, username, role, expiration } = response.data;
      
      // Guardar datos en localStorage
      localStorage.setItem('token', token);
      localStorage.setItem('user', JSON.stringify({
        username,
        role,
        expiration
      }));
      
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al iniciar sesión'
      );
    }
  },

  // Logout del usuario
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  // Verificar si el usuario está autenticado
  isAuthenticated() {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    
    if (!token || !user) return false;
    
    try {
      const userData = JSON.parse(user);
      const expirationDate = new Date(userData.expiration);
      const now = new Date();
      
      return expirationDate > now;
    } catch {
      return false;
    }
  },

  // Obtener datos del usuario actual
  getCurrentUser() {
    try {
      const user = localStorage.getItem('user');
      return user ? JSON.parse(user) : null;
    } catch {
      return null;
    }
  },

  // Verificar si el usuario es admin
  isAdmin() {
    const user = this.getCurrentUser();
    return user?.role === 'Admin';
  }
};

export { apiClient };
