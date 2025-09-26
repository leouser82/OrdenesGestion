import React, { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { ordenesService } from '../services/ordenesService';
import { 
  RefreshCw, 
  Play, 
  X, 
  AlertCircle, 
  CheckCircle,
  Clock,
  DollarSign,
  TrendingUp,
  TrendingDown
} from 'lucide-react';

const OrdenesPage = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [actionLoading, setActionLoading] = useState(null);
  const { isAdmin } = useAuth();

  useEffect(() => {
    loadOrdenes();
  }, []);

  const loadOrdenes = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await ordenesService.getAllOrdenes();
      setOrdenes(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleEjecutar = async (id) => {
    try {
      setActionLoading(id);
      await ordenesService.ejecutarOrden(id);
      await loadOrdenes(); // Recargar la lista
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(null);
    }
  };

  const handleCancelar = async (id) => {
    if (!window.confirm('¿Está seguro de que desea cancelar esta orden?')) {
      return;
    }

    try {
      setActionLoading(id);
      await ordenesService.cancelarOrden(id);
      await loadOrdenes(); // Recargar la lista
    } catch (err) {
      setError(err.message);
    } finally {
      setActionLoading(null);
    }
  };

  const getStatusIcon = (estadoId) => {
    switch (estadoId) {
      case 1: return <Clock size={16} />;
      case 2: return <CheckCircle size={16} />;
      case 3: return <X size={16} />;
      default: return <Clock size={16} />;
    }
  };

  const getStatusClass = (estadoId) => {
    switch (estadoId) {
      case 1: return 'status-en-proceso';
      case 2: return 'status-ejecutada';
      case 3: return 'status-cancelada';
      default: return 'status-en-proceso';
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-CO', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 2
    }).format(amount || 0);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('es-CO', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return (
      <div className="container">
        <div className="loading">
          <div className="spinner"></div>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <div className="card">
        <div style={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center',
          marginBottom: '24px'
        }}>
          <h1 style={{ color: '#374151', margin: 0 }}>
            Órdenes de Inversión
          </h1>
          <button
            onClick={loadOrdenes}
            className="btn btn-secondary"
            disabled={loading}
          >
            <RefreshCw size={16} />
            Actualizar
          </button>
        </div>

        {error && (
          <div className="alert alert-error">
            <AlertCircle size={16} style={{ marginRight: '8px' }} />
            {error}
          </div>
        )}

        {ordenes.length === 0 ? (
          <div style={{ 
            textAlign: 'center', 
            padding: '40px',
            color: '#6b7280'
          }}>
            <TrendingUp size={48} style={{ marginBottom: '16px' }} />
            <p>No hay órdenes de inversión registradas</p>
          </div>
        ) : (
          <div style={{ overflowX: 'auto' }}>
            <table className="table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Cuenta</th>
                  <th>Activo</th>
                  <th>Operación</th>
                  <th>Cantidad</th>
                  <th>Monto Total</th>
                  <th>Estado</th>
                  <th>Fecha</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {ordenes.map((orden) => (
                  <tr key={orden.id}>
                    <td>#{orden.id}</td>
                    <td>{orden.cuentaId}</td>
                    <td>
                      <div>
                        <strong>{orden.activoTicker}</strong>
                        <br />
                        <small style={{ color: '#6b7280' }}>
                          {orden.activoNombre}
                        </small>
                      </div>
                    </td>
                    <td>
                      <div style={{ 
                        display: 'flex', 
                        alignItems: 'center', 
                        gap: '4px',
                        color: orden.operacion === 'C' ? '#059669' : '#dc2626'
                      }}>
                        {orden.operacion === 'C' ? (
                          <TrendingUp size={16} />
                        ) : (
                          <TrendingDown size={16} />
                        )}
                        {orden.operacion === 'C' ? 'Compra' : 'Venta'}
                      </div>
                    </td>
                    <td>{orden.cantidad.toLocaleString()}</td>
                    <td>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                        <DollarSign size={14} />
                        {formatCurrency(orden.montoTotal)}
                      </div>
                    </td>
                    <td>
                      <span className={`status-badge ${getStatusClass(orden.estadoId)}`}>
                        <span style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                          {getStatusIcon(orden.estadoId)}
                          {orden.estadoDescripcion}
                        </span>
                      </span>
                    </td>
                    <td>
                      <small>
                        {formatDate(orden.fechaCreacion)}
                      </small>
                    </td>
                    <td>
                      <div style={{ display: 'flex', gap: '8px' }}>
                        {orden.estadoId === 1 && (
                          <button
                            onClick={() => handleEjecutar(orden.id)}
                            className="btn btn-primary"
                            style={{ padding: '6px 12px', fontSize: '12px' }}
                            disabled={actionLoading === orden.id}
                          >
                            {actionLoading === orden.id ? (
                              <div className="spinner" style={{ width: '12px', height: '12px' }}></div>
                            ) : (
                              <>
                                <Play size={12} />
                                Ejecutar
                              </>
                            )}
                          </button>
                        )}
                        
                        {isAdmin() && orden.estadoId === 1 && (
                          <button
                            onClick={() => handleCancelar(orden.id)}
                            className="btn btn-danger"
                            style={{ padding: '6px 12px', fontSize: '12px' }}
                            disabled={actionLoading === orden.id}
                          >
                            {actionLoading === orden.id ? (
                              <div className="spinner" style={{ width: '12px', height: '12px' }}></div>
                            ) : (
                              <>
                                <X size={12} />
                                Cancelar
                              </>
                            )}
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default OrdenesPage;
