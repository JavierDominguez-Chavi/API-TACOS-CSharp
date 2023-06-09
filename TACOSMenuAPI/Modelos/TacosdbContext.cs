using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TACOSMenuAPI.Modelos;

/// <summary>
/// Punto de acceso a la base de datos mediante EntityFrameworkCore.
/// </summary>
public partial class TacosdbContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    public TacosdbContext()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public TacosdbContext(DbContextOptions<TacosdbContext> options)
        : base(options)
    {
    }
    /// <summary>
    /// Alimentos en la base de datos.
    /// </summary>
    public virtual DbSet<Alimento> Alimentos { get; set; }
    /// <summary>
    /// Imagenes en la base de datos.
    /// </summary>
    public virtual DbSet<Imagen> Imagenes { get; set; }

    /// <summary>
    /// Modelado code-first de la base de datos relacional.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alimento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alimentos");

            entity.HasIndex(e => e.IdCategoria, "idCategoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .HasColumnName("descripcion");
            entity.Property(e => e.Existencia).HasColumnName("existencia");
            entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");
            entity.Property(e => e.IdImagen)
                .HasColumnName("idImagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.HasOne(d => d.Imagen).WithMany(p => p.Alimentos)
                .HasForeignKey(d => d.IdImagen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alimentos_ibfk_2");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("imagenes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImagenBytes).HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
