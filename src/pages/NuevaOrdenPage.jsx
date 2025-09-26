import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { ordenesService, referenceDataService } from '../services/ordenesService';
import { 
  Plus, 
  AlertCircle, 
  CheckCircle, 
  DollarSign,
  TrendingUp,
  TrendingDown,
  Building,
  Hash
} from 'lucide-react';

const NuevaOrdenPage = () => {
  const [formData, setFormData] = useState({
    cuentaId: '',
    activoId: '',
    cantidad: '',
    operacion: 'C'
  });
  
  const [activos, setActivos] = useState([]);
  const [loading, setLoading] = useState(false);
  const [loadingData, setLoadingData] = useState(true);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  
  const navigate = useNavigate();

  useEffect(() => {
    loadReferenceData();
  }, []);

  const loadReferenceData = async () => {
    try {
      setLoadingData(true);
      const activosData = await referenceDataService.getActivosFinancieros();
      setActivos(activosData);
    } catch (err) {
      setError('Error al cargar los datos: ' + err.message);
    } finally {
      setLoadingData(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    
    // Limpiar mensajes cuando el usuario empiece a escribir
    if (error) setError('');
    if (success) setSuccess('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    setSuccess('');

    try {
      // Validar datos
      if (!formData.cuentaId || !formData.activoId || !formData.cantidad) {
        throw new Error('Todos los campos son obligatorios');
      }

      if (parseInt(formData.cantidad) <= 0) {
        throw new Error('La cantidad debe ser mayor a 0');
      }

      const ordenData = {
        cuentaId: parseInt(formData.cuentaId),
        activoId: parseInt(formData.activoId),
        cantidad: parseInt(formData.cantidad),
        operacion: formData.operacion
      };

      await ordenesService.createOrden(ordenData);
      setSuccess('Orden creada exitosamente');
      
      // Limpiar formulario
      setFormData({
        cuentaId: '',
        activoId: '',
        cantidad: '',
        operacion: 'C'
      });

      // Redirigir después de 2 segundos
      setTimeout(() => {
        navigate('/ordenes');
      }, 2000);

    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const getSelectedActivo = () => {
    return activos.find(activo => activo.id === parseInt(formData.activoId));
  };

  const calculateTotal = () => {
    const activo = getSelectedActivo();
    if (activo && formData.cantidad && activo.precioUnitario) {
      return activo.precioUnitario * parseInt(formData.cantidad);
    }
    return 0;
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 2
    }).format(amount);
  };

  if (loadingData) {
    return (
      <div className="container">
        <div className="loading">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  return (
    <div className="container" style={{ maxWidth: '600px' }}>
      <div className="card">
        <div style={{ textAlign: 'center', marginBottom: '32px' }}>
          <Plus size={48} style={{ color: '#667eea', marginBottom: '16px' }} />
          <h1 style={{ color: '#374151', marginBottom: '8px' }}>
            Nueva Orden de Inversión
          </h1>
          <p style={{ color: '#6b7280' }}>
            Complete los datos para crear una nueva orden
          </p>
        </div>

        {error && (
          <div className="alert alert-error">
            <AlertCircle size={16} style={{ marginRight: '8px' }} />
            {error}
          </div>
        )}

        {success && (
          <div className="alert alert-success">
            <CheckCircle size={16} style={{ marginRight: '8px' }} />
            {success}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="cuentaId">
              <Building size={16} style={{ marginRight: '8px', display: 'inline' }} />
              ID de Cuenta
            </label>
            <input
              type="number"
              id="cuentaId"
              name="cuentaId"
              value={formData.cuentaId}
              onChange={handleChange}
              className="form-control"
              placeholder="Ej: 12345"
              required
              disabled={loading}
              min="1"
            />
          </div>

          <div className="form-group">
            <label htmlFor="activoId">
              <Hash size={16} style={{ marginRight: '8px', display: 'inline' }} />
              Activo Financiero
            </label>
            <select
              id="activoId"
              name="activoId"
              value={formData.activoId}
              onChange={handleChange}
              className="form-control"
              required
              disabled={loading}
            >
              <option value="">Seleccione un activo</option>
              {activos.map(activo => (
                <option key={activo.id} value={activo.id}>
                  {activo.ticker} - {activo.nombre} 
                  {activo.precioUnitario && ` (${formatCurrency(activo.precioUnitario)})`}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label htmlFor="cantidad">
              <Hash size={16} style={{ marginRight: '8px', display: 'inline' }} />
              Cantidad
            </label>
            <input
              type="number"
              id="cantidad"
              name="cantidad"
              value={formData.cantidad}
              onChange={handleChange}
              className="form-control"
              placeholder="Ej: 10"
              required
              disabled={loading}
              min="1"
            />
          </div>

          <div className="form-group">
            <label htmlFor="operacion">
              Tipo de Operación
            </label>
            <div style={{ display: 'flex', gap: '16px', marginTop: '8px' }}>
              <label style={{ 
                display: 'flex', 
                alignItems: 'center', 
                cursor: 'pointer',
                padding: '12px 16px',
                border: formData.operacion === 'C' ? '2px solid #059669' : '2px solid #e5e7eb',
                borderRadius: '8px',
                backgroundColor: formData.operacion === 'C' ? '#f0fdf4' : '#ffffff',
                transition: 'all 0.2s'
              }}>
                <input
                  type="radio"
                  name="operacion"
                  value="C"
                  checked={formData.operacion === 'C'}
                  onChange={handleChange}
                  disabled={loading}
                  style={{ marginRight: '8px' }}
                />
                <TrendingUp size={16} style={{ marginRight: '8px', color: '#059669' }} />
                Compra
              </label>
              
              <label style={{ 
                display: 'flex', 
                alignItems: 'center', 
                cursor: 'pointer',
                padding: '12px 16px',
                border: formData.operacion === 'V' ? '2px solid #dc2626' : '2px solid #e5e7eb',
                borderRadius: '8px',
                backgroundColor: formData.operacion === 'V' ? '#fef2f2' : '#ffffff',
                transition: 'all 0.2s'
              }}>
                <input
                  type="radio"
                  name="operacion"
                  value="V"
                  checked={formData.operacion === 'V'}
                  onChange={handleChange}
                  disabled={loading}
                  style={{ marginRight: '8px' }}
                />
                <TrendingDown size={16} style={{ marginRight: '8px', color: '#dc2626' }} />
                Venta
              </label>
            </div>
          </div>

          {formData.activoId && formData.cantidad && (
            <div style={{
              background: '#f8fafc',
              border: '1px solid #e2e8f0',
              borderRadius: '8px',
              padding: '16px',
              marginBottom: '24px'
            }}>
              <h3 style={{ color: '#374151', marginBottom: '12px', fontSize: '16px' }}>
                Resumen de la Orden
              </h3>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ color: '#6b7280' }}>Monto Total Estimado:</span>
                <span style={{ 
                  fontSize: '18px', 
                  fontWeight: '600', 
                  color: '#374151',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '4px'
                }}>
                  <DollarSign size={16} />
                  {formatCurrency(calculateTotal())}
                </span>
              </div>
            </div>
          )}

          <div style={{ display: 'flex', gap: '12px' }}>
            <button
              type="button"
              onClick={() => navigate('/ordenes')}
              className="btn btn-secondary"
              style={{ flex: 1 }}
              disabled={loading}
            >
              Cancelar
            </button>
            
            <button 
              type="submit" 
              className="btn btn-primary" 
              style={{ flex: 2 }}
              disabled={loading}
            >
              {loading ? (
                <div className="spinner" style={{ width: '20px', height: '20px' }}></div>
              ) : (
                <>
                  <Plus size={16} />
                  Crear Orden
                </>
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default NuevaOrdenPage;
