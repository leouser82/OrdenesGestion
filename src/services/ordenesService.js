import { apiClient } from './authService';

export const ordenesService = {
  // Obtener todas las órdenes
  async getAllOrdenes() {
    try {
      const response = await apiClient.get('/ordenesinversion');
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al obtener las órdenes'
      );
    }
  },

  // Obtener orden por ID
  async getOrdenById(id) {
    try {
      const response = await apiClient.get(`/ordenesinversion/${id}`);
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al obtener la orden'
      );
    }
  },

  // Crear nueva orden
  async createOrden(ordenData) {
    try {
      const response = await apiClient.post('/ordenesinversion', ordenData);
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al crear la orden'
      );
    }
  },

  // Ejecutar orden
  async ejecutarOrden(id) {
    try {
      const response = await apiClient.put(`/ordenesinversion/${id}/ejecutar`);
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al ejecutar la orden'
      );
    }
  },

  // Cancelar orden (solo Admin)
  async cancelarOrden(id) {
    try {
      const response = await apiClient.delete(`/ordenesinversion/${id}/cancelar`);
      return response.data;
    } catch (error) {
      throw new Error(
        error.response?.data?.message || 
        'Error al cancelar la orden'
      );
    }
  }
};

export const referenceDataService = {
  // Obtener estados de orden
  async getEstadosOrden() {
    try {
      const response = await apiClient.get('/referencedata/estados-orden');
      return response.data;
    } catch (error) {
      throw new Error('Error al obtener estados de orden');
    }
  },

  // Obtener tipos de activo
  async getTiposActivo() {
    try {
      const response = await apiClient.get('/referencedata/tipos-activo');
      return response.data;
    } catch (error) {
      throw new Error('Error al obtener tipos de activo');
    }
  },

  // Obtener activos financieros
  async getActivosFinancieros() {
    try {
      const response = await apiClient.get('/referencedata/activos-financieros');
      return response.data;
    } catch (error) {
      throw new Error('Error al obtener activos financieros');
    }
  },

  // Obtener activos financieros por tipo
  async getActivosPorTipo(tipoId) {
    try {
      const response = await apiClient.get(`/referencedata/activos-financieros/por-tipo/${tipoId}`);
      return response.data;
    } catch (error) {
      throw new Error('Error al obtener activos por tipo');
    }
  }
};
