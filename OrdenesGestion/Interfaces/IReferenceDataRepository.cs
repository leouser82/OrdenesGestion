using GestionOrdenes.Models;

namespace GestionOrdenes.Interfaces
{
    public interface IReferenceDataRepository
    {


        // Tipos de Activo
        Task<IEnumerable<ActivoFinanciero>> GetAllActivosFinancierosAsync();
        Task<IEnumerable<EstadoOrden>> GetAllEstadosOrdenAsync();
        Task<IEnumerable<TipoActivo>> GetAllTiposActivoAsync();
        Task<EstadoOrden> GetEstadoOrdenByIdAsync(int id);

    }
}


