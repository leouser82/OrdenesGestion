using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdenes.Models
{
    public class OrdenInversion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CuentaId { get; set; }

        [Required]
        public int ActivoId { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Precio { get; set; }

        [Required]
        [StringLength(1)]
        public string Operacion { get; set; } = string.Empty; // C = Compra, V = Venta

        [Required]
        public int EstadoId { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? MontoTotal { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Comision { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Impuesto { get; set; }

        // Navigation properties
        [ForeignKey("ActivoId")]
        public virtual ActivoFinanciero ActivoFinanciero { get; set; } = null!;

        [ForeignKey("EstadoId")]
        public virtual EstadoOrden EstadoOrden { get; set; } = null!;
    }
}