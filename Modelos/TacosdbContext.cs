﻿#pragma warning disable CS1591
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TACOS.Modelos;

/// <summary>
/// Punto de acceso a la base de datos mediante EntityFrameworkCore.
/// </summary>
public partial class TacosdbContext : DbContext
{
    public TacosdbContext()
    {
    }

    public TacosdbContext(DbContextOptions<TacosdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alimento> Alimentos { get; set; }

    public virtual DbSet<Alimentospedido> Alimentospedidos { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Miembro> Miembros { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Resena> Resenas { get; set; }
    public virtual DbSet<Puesto> Puestos { get; set; }
    public virtual DbSet<Turno> Turnos { get; set; }
    public virtual DbSet<Staff> Staff { get; set; }

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

            entity.HasOne(d => d.Categoria).WithMany(p => p.Alimentos)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alimentos_ibfk_1");

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
            entity.Property(e => e.Nombre)
                .HasMaxLength(1000)
                .HasColumnName("nombre");
            entity.Property(e => e.ImagenBytes)
                .HasColumnName("imagen")
                .HasColumnType("longblob");
        });

        modelBuilder.Entity<Alimentospedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alimentospedidos");

            entity.HasIndex(e => e.IdAlimento, "idAlimento");

            entity.HasIndex(e => e.IdPedido, "idPedido");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdAlimento).HasColumnName("idAlimento");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");

            entity.HasOne(d => d.Alimento).WithMany(p => p.Alimentospedidos)
                .HasForeignKey(d => d.IdAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alimentospedidos_ibfk_1");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Alimentospedidos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alimentospedidos_ibfk_2");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Medida)
                .HasMaxLength(30)
                .HasColumnName("medida");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Miembro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("miembros");

            entity.HasIndex(e => e.IdPersona, "idPersona");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.CodigoConfirmacion).HasColumnName("codigoConfirmacion");
            entity.Property(e => e.PedidosPagados).HasColumnName("pedidosPagados");

            entity.HasOne(d => d.Persona).WithMany(p => p.Miembros)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("miembros_ibfk_1");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedidos");

            entity.HasIndex(e => e.IdMiembro, "idMiembro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdMiembro).HasColumnName("idMiembro");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.Miembro).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdMiembro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pedidos_ibfk_1");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("personas");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(255)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(255)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .HasColumnName("direccion");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(255)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("resenas");

            entity.HasIndex(e => e.IdMiembro, "idMiembro");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .HasColumnName("descripcion");
            entity.Property(e => e.Imagen)
                .HasColumnType("mediumblob")
                .HasColumnName("imagen");
            entity.Property(e => e.Fecha).HasColumnName("fecha");

            entity.HasOne(d => d.Miembro).WithMany(p => p.Resenas)
                .HasForeignKey(d => d.IdMiembro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("resenas_ibfk_1");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("staff");

            entity.HasIndex(e => e.IdPersona, "idPersona");
            entity.HasIndex(e => e.IdPuesto, "idPuesto");
            entity.HasIndex(e => e.IdTurno, "idTurno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .HasColumnName("contrasena");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.IdPuesto).HasColumnName("idPuesto");
            entity.Property(e => e.IdTurno).HasColumnName("idTurno");

            entity.HasOne(d => d.Persona).WithMany(p => p.Staff)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("staff_ibfk_1");
            entity.HasOne(d => d.Puesto).WithMany(p => p.Staff)
                .HasForeignKey(d => d.IdPuesto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("staff_ibfk_2");
            entity.HasOne(d => d.Turno).WithMany(p => p.Staff)
            .HasForeignKey(d => d.IdTurno)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("staff_ibfk_3");
        });

        modelBuilder.Entity<Puesto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("puestos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cargo).HasColumnName("cargo");
           
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("turnos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.HoraInicio).HasColumnName("horaInicio");
            entity.Property(e => e.HoraFin).HasColumnName("horaFin");

        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
