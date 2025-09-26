using System.ComponentModel.DataAnnotations;

namespace GestionOrdenes.DTOs
{
    public class ActivoFinancieroDto
    {
        public int Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int TipoActivoId { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public string? TipoActivoDescripcion { get; set; }
    }

    public class UpdatePrecioActivoDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }
    }
}