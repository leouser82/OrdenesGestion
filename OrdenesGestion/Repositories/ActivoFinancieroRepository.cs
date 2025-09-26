using Microsoft.EntityFrameworkCore;
using GestionOrdenes.Data;
using GestionOrdenes.Interfaces;
using GestionOrdenes.Models;

namespace GestionOrdenes.Repositories
{
    public class ActivoFinancieroRepository : Repository<ActivoFinanciero>, IActivoFinancieroRepository
    {
        public ActivoFinancieroRepository(GestionOrdenesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ActivoFinanciero>> GetActivosByTipoAsync(int tipoActivoId)
        {
            return await _dbSet
                .Include(a => a.TipoActivo)
                .Where(a => a.TipoActivoId == tipoActivoId)
                .ToListAsync();
        }

        public async Task<ActivoFinanciero?> GetByTickerAsync(string ticker)
        {
            return await _dbSet
                .Include(a => a.TipoActivo)
                .FirstOrDefaultAsync(a => a.Ticker == ticker);
        }

        public async Task<ActivoFinanciero?> GetActivoWithTipoAsync(int id)
        {
            return await _dbSet
                .Include(a => a.TipoActivo)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<ActivoFinanciero>> GetAllWithTiposAsync()
        {
            return await _dbSet
                .Include(a => a.TipoActivo)
                .ToListAsync();
        }
    }
}