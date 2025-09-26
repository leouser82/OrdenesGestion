using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdenes.Models
{
    public class ActivoFinanciero
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Ticker { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int TipoActivoId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? PrecioUnitario { get; set; }

        // Navigation properties
        [ForeignKey("TipoActivoId")]
        public virtual TipoActivo TipoActivo { get; set; } = null!;

        public virtual ICollection<OrdenInversion> OrdenesInversion { get; set; } = new List<OrdenInversion>();
    }
}