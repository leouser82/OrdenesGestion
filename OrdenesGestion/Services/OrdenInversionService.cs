using GestionOrdenes.DTOs;
using GestionOrdenes.Interfaces;
using GestionOrdenes.Models;



namespace GestionOrdenes.Services
{
    public class OrdenInversionService : IOrdenInversionService
    {
        private readonly IOrdenInversionRepository _ordenRepository;
        private readonly IActivoFinancieroRepository _activoRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public OrdenInversionService(
            IOrdenInversionRepository ordenRepository,
            IActivoFinancieroRepository activoRepository,
            IReferenceDataRepository referenceDataRepository)
        {
            _ordenRepository = ordenRepository;
            _activoRepository = activoRepository;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IEnumerable<OrdenInversionDto>> GetAllOrdenesAsync()
        {
            var ordenes = await _ordenRepository.GetAllWithDetailsAsync();
            return ordenes.Select(MapToDto);
        }

        public async Task<OrdenInversionDto?> GetOrdenByIdAsync(int id)
        {
            var orden = await _ordenRepository.GetOrdenWithDetailsAsync(id);
            return orden != null ? MapToDto(orden) : null;
        }

        public async Task<OrdenInversionDto> CreateOrdenAsync(CreateOrdenInversionDto createDto)
        {
            // Validar que el activo existe
            var activo = await _activoRepository.GetActivoWithTipoAsync(createDto.ActivoId);
            if (activo == null)
            {
                throw new ArgumentException("El activo financiero no existe", nameof(createDto.ActivoId));
            }

            // Validar que el estado existe (por defecto será "En proceso")
            var estado = await _referenceDataRepository.GetEstadoOrdenByIdAsync(1); // En proceso
            if (estado == null)
            {
                throw new InvalidOperationException("Estado 'En proceso' no encontrado");
            }

            var orden = new OrdenInversion
            {
                CuentaId = createDto.CuentaId,
                ActivoId = createDto.ActivoId,
                Cantidad = createDto.Cantidad,
                Precio = activo.PrecioUnitario.Value,
                Operacion = createDto.Operacion,
                EstadoId = 1 // En proceso
            };

            // Calcular montos
            CalcularMontos(orden, activo);

            await _ordenRepository.AddAsync(orden); // Auto-save incluido

            // Obtener la orden completa con detalles
            var ordenCompleta = await _ordenRepository.GetOrdenWithDetailsAsync(orden.Id);
            return MapToDto(ordenCompleta!);
        }

        public async Task<OrdenInversionDto?> UpdateEstadoOrdenAsync(int id, UpdateEstadoOrdenDto updateDto)
        {
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                return null;
            }

            // Validar que el nuevo estado existe
            var estado = await _referenceDataRepository.GetEstadoOrdenByIdAsync(updateDto.EstadoId);
            if (estado == null)
            {
                throw new ArgumentException("El estado no existe", nameof(updateDto.EstadoId));
            }

            orden.EstadoId = updateDto.EstadoId;
            await _ordenRepository.UpdateAsync(orden); // Auto-save incluido

            var ordenActualizada = await _ordenRepository.GetOrdenWithDetailsAsync(id);
            return MapToDto(ordenActualizada!);
        }

        public async Task<OrdenInversionDto?> EjecutarOrdenAsync(int id)
        {
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                return null;
            }

            // Validar que el estado "Ejecutada" existe
            var estadoEjecutada = await _referenceDataRepository.GetEstadoOrdenByIdAsync(2);
            if (estadoEjecutada == null)
            {
                throw new InvalidOperationException("Estado 'Ejecutada' no encontrado");
            }

            // Cambiar estado a "Ejecutada" (ID = 2)
            orden.EstadoId = 2;
            await _ordenRepository.UpdateAsync(orden); // Auto-save incluido

            var ordenActualizada = await _ordenRepository.GetOrdenWithDetailsAsync(id);
            return MapToDto(ordenActualizada!);
        }

        public async Task<bool> CancelarOrdenAsync(int id)
        {
            var orden = await _ordenRepository.GetByIdAsync(id);
            if (orden == null)
            {
                return false;
            }

            // Validar que el estado "Cancelada" existe
            var estadoCancelada = await _referenceDataRepository.GetEstadoOrdenByIdAsync(3);
            if (estadoCancelada == null)
            {
                throw new InvalidOperationException("Estado 'Cancelada' no encontrado");
            }

            // Soft delete: cambiar estado a "Cancelada" (ID = 3)
            orden.EstadoId = 3;
            await _ordenRepository.UpdateAsync(orden); // Auto-save incluido
            return true;
        }

        private void CalcularMontos(OrdenInversion orden, ActivoFinanciero activo)
        {
            var montoTotal = orden.Precio * orden.Cantidad;
            orden.MontoTotal = montoTotal;

            // Calcular comisión si aplica
            if (activo.TipoActivo.PorcentajeComision.HasValue)
            {
                orden.Comision = montoTotal * (activo.TipoActivo.PorcentajeComision.Value / 100);
            }

            // Calcular impuesto si aplica (solo para ventas)
            if (orden.Operacion == "V" && activo.TipoActivo.PorcentajeImpuesto.HasValue)
            {
                orden.Impuesto = montoTotal * (activo.TipoActivo.PorcentajeImpuesto.Value / 100.0m);
            }
        }

        private OrdenInversionDto MapToDto(OrdenInversion orden)
        {
            return new OrdenInversionDto
            {
                Id = orden.Id,
                CuentaId = orden.CuentaId,
                ActivoId = orden.ActivoId,
                Cantidad = orden.Cantidad,
                Operacion = orden.Operacion,
                EstadoId = orden.EstadoId,
                MontoTotal = orden.MontoTotal,
                Comision = orden.Comision,
                Impuesto = orden.Impuesto,
                ActivoNombre = orden.ActivoFinanciero?.Nombre,
                ActivoTicker = orden.ActivoFinanciero?.Ticker,
                EstadoDescripcion = orden.EstadoOrden?.DescripcionEstado
            };
        }
    }
}
