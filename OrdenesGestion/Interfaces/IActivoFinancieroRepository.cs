using GestionOrdenes.Models;

namespace GestionOrdenes.Interfaces
{
    public interface IActivoFinancieroRepository : IRepository<ActivoFinanciero>
    {
        Task<IEnumerable<ActivoFinanciero>> GetActivosByTipoAsync(int tipoActivoId);
        Task<ActivoFinanciero?> GetByTickerAsync(string ticker);
        Task<ActivoFinanciero?> GetActivoWithTipoAsync(int id);
        Task<IEnumerable<ActivoFinanciero>> GetAllWithTiposAsync();
    }
}