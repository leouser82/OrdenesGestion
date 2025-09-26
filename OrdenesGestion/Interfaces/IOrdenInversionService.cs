using GestionOrdenes.DTOs;

namespace GestionOrdenes.Interfaces
{
    public interface IOrdenInversionService
    {
        Task<IEnumerable<OrdenInversionDto>> GetAllOrdenesAsync();
        Task<OrdenInversionDto?> GetOrdenByIdAsync(int id);
        Task<OrdenInversionDto> CreateOrdenAsync(CreateOrdenInversionDto createDto);
        Task<OrdenInversionDto?> UpdateEstadoOrdenAsync(int id, UpdateEstadoOrdenDto updateDto);
        Task<OrdenInversionDto?> EjecutarOrdenAsync(int id);
        Task<bool> CancelarOrdenAsync(int id);
    }
}