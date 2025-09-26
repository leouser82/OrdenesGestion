using Microsoft.EntityFrameworkCore;
using GestionOrdenes.Data;
using GestionOrdenes.Interfaces;
using GestionOrdenes.Models;

namespace GestionOrdenes.Repositories
{
    public class OrdenInversionRepository : Repository<OrdenInversion>, IOrdenInversionRepository
    {
        public OrdenInversionRepository(GestionOrdenesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrdenInversion>> GetOrdenesByCuentaAsync(int cuentaId)
        {
            return await _dbSet
                .Include(o => o.ActivoFinanciero)
                    .ThenInclude(a => a.TipoActivo)
                .Include(o => o.EstadoOrden)
                .Where(o => o.CuentaId == cuentaId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrdenInversion>> GetOrdenesByActivoAsync(int activoId)
        {
            return await _dbSet
                .Include(o => o.ActivoFinanciero)
                    .ThenInclude(a => a.TipoActivo)
                .Include(o => o.EstadoOrden)
                .Where(o => o.ActivoId == activoId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrdenInversion>> GetOrdenesByEstadoAsync(int estadoId)
        {
            return await _dbSet
                .Include(o => o.ActivoFinanciero)
                    .ThenInclude(a => a.TipoActivo)
                .Include(o => o.EstadoOrden)
                .Where(o => o.EstadoId == estadoId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<OrdenInversion?> GetOrdenWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.ActivoFinanciero)
                    .ThenInclude(a => a.TipoActivo)
                .Include(o => o.EstadoOrden)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OrdenInversion>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(o => o.ActivoFinanciero)
                    .ThenInclude(a => a.TipoActivo)
                .Include(o => o.EstadoOrden)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }
    }
}