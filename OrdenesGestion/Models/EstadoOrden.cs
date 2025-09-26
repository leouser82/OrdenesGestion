using System.ComponentModel.DataAnnotations;

namespace GestionOrdenes.Models
{
    public class EstadoOrden
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DescripcionEstado { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<OrdenInversion> OrdenesInversion { get; set; } = new List<OrdenInversion>();
    }
}