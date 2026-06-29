using System;
using System.Collections.Generic;
using Backend.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Colore> Colores { get; set; }

    public virtual DbSet<DetalleOrden> DetalleOrdens { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Sneaker> Sneakers { get; set; }

    public virtual DbSet<Talla> Tallas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_categorias");
        });

        modelBuilder.Entity<Colore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_colores");
        });

        modelBuilder.Entity<DetalleOrden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_detalle");

            entity.Property(e => e.Cantidad).HasDefaultValue(1);

            entity.HasOne(d => d.IdInventarioNavigation).WithMany(p => p.DetalleOrdens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_det_inv");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.DetalleOrdens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_det_orden");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_inventario");

            entity.HasOne(d => d.IdColorNavigation).WithMany(p => p.Inventarios).HasConstraintName("fk_inv_color");

            entity.HasOne(d => d.IdSneakerNavigation).WithMany(p => p.Inventarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inv_sne");

            entity.HasOne(d => d.IdTallaNavigation).WithMany(p => p.Inventarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inv_talla");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_ordenes");

            entity.Property(e => e.Estado).HasDefaultValue("Procesando");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Ordenes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ord_user");
        });

        modelBuilder.Entity<Sneaker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_sneakers");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Sneakers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sne_cat");
        });

        modelBuilder.Entity<Talla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tallas");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_usuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
