using Microsoft.EntityFrameworkCore;
using GestionOrdenes.Data;
using GestionOrdenes.Interfaces;
using GestionOrdenes.Models;

namespace GestionOrdenes.Repositories
{
    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly GestionOrdenesDbContext _context;

        public ReferenceDataRepository(GestionOrdenesDbContext context)
        {
            _context = context;
        }

        // Tipos de Activo
        public async Task<IEnumerable<TipoActivo>> GetAllTiposActivoAsync()
        {
            return await _context.TiposActivo
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .ToListAsync();
        }


        // Activos Financieros
        public async Task<IEnumerable<ActivoFinanciero>> GetAllActivosFinancierosAsync()
        {
            return await _context.ActivosFinancieros
                .AsNoTracking()
                .Include(a => a.TipoActivo)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<EstadoOrden>> GetAllEstadosOrdenAsync()
        {
            return await _context.EstadosOrden
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .ToListAsync();

        }

        public async Task<EstadoOrden> GetEstadoOrdenByIdAsync(int id)
        {
            return await _context.EstadosOrden
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .FirstOrDefaultAsync(a => a.Id == id);

        }
    }
}


