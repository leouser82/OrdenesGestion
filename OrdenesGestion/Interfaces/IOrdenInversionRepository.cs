using GestionOrdenes.Models;

namespace GestionOrdenes.Interfaces
{
    public interface IOrdenInversionRepository : IRepository<OrdenInversion>
    {
        Task<IEnumerable<OrdenInversion>> GetOrdenesByCuentaAsync(int cuentaId);
        Task<IEnumerable<OrdenInversion>> GetOrdenesByActivoAsync(int activoId);
        Task<IEnumerable<OrdenInversion>> GetOrdenesByEstadoAsync(int estadoId);
        Task<OrdenInversion?> GetOrdenWithDetailsAsync(int id);
        Task<IEnumerable<OrdenInversion>> GetAllWithDetailsAsync();
    }
}