using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdenes.Models
{
    public class TipoActivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,4)")]
        public decimal? PorcentajeComision { get; set; }

        public int? PorcentajeImpuesto { get; set; }

        // Navigation property
        public virtual ICollection<ActivoFinanciero> ActivosFinancieros { get; set; } = new List<ActivoFinanciero>();
    }
}