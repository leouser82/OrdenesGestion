using Microsoft.EntityFrameworkCore;
using GestionOrdenes.Models;

namespace GestionOrdenes.Data
{
    public class GestionOrdenesDbContext : DbContext
    {
        public GestionOrdenesDbContext(DbContextOptions<GestionOrdenesDbContext> options) : base(options)
        {
        }

        public DbSet<TipoActivo> TiposActivo { get; set; }
        public DbSet<EstadoOrden> EstadosOrden { get; set; }
        public DbSet<ActivoFinanciero> ActivosFinancieros { get; set; }
        public DbSet<OrdenInversion> OrdenesInversion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de TipoActivo
            modelBuilder.Entity<TipoActivo>(entity =>
            {
                entity.ToTable("TipoActivo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PorcentajeComision).HasColumnType("decimal(5,4)");
            });

            // Configuración de EstadoOrden
            modelBuilder.Entity<EstadoOrden>(entity =>
            {
                entity.ToTable("EstadoOrden");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.DescripcionEstado).IsRequired().HasMaxLength(50);
            });

            // Configuración de ActivoFinanciero
            modelBuilder.Entity<ActivoFinanciero>(entity =>
            {
                entity.ToTable("ActivoFinanciero");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Ticker).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18,4)");
                
                entity.HasOne(d => d.TipoActivo)
                    .WithMany(p => p.ActivosFinancieros)
                    .HasForeignKey(d => d.TipoActivoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de OrdenInversion
            modelBuilder.Entity<OrdenInversion>(entity =>
            {
                entity.ToTable("OrdenInversion");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.Property(e => e.Precio).HasColumnType("decimal(18,4)");
                entity.Property(e => e.MontoTotal).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Comision).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Impuesto).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Operacion).HasMaxLength(1);

                entity.HasOne(d => d.ActivoFinanciero)
                    .WithMany(p => p.OrdenesInversion)
                    .HasForeignKey(d => d.ActivoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.EstadoOrden)
                    .WithMany(p => p.OrdenesInversion)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}