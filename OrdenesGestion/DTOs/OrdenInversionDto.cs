using System.ComponentModel.DataAnnotations;

namespace GestionOrdenes.DTOs
{
    public class OrdenInversionDto
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public int ActivoId { get; set; }
        public int Cantidad { get; set; }
        public string Operacion { get; set; } = string.Empty;
        public int EstadoId { get; set; }
        public decimal? MontoTotal { get; set; }
        public decimal? Comision { get; set; }
        public decimal? Impuesto { get; set; }
        public string? ActivoNombre { get; set; }
        public string? ActivoTicker { get; set; }
        public string? EstadoDescripcion { get; set; }
    }

    public class CreateOrdenInversionDto
    {
        [Required]
        public int CuentaId { get; set; }

        [Required]
        public int ActivoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required]
        [RegularExpression("^[CV]$", ErrorMessage = "La operación debe ser 'C' (Compra) o 'V' (Venta)")]
        public string Operacion { get; set; } = string.Empty;
    }

    public class UpdateEstadoOrdenDto
    {
        [Required]
        [Range(1, 3, ErrorMessage = "El estado debe ser 1 (En proceso), 2 (Ejecutada) o 3 (Cancelada)")]
        public int EstadoId { get; set; }
    }
}