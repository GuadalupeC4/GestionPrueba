using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GestionPrueba.Models;

namespace GestionPrueba.Models;

public partial class GestionPContext : DbContext
{
    public GestionPContext()
    {
    }

    public GestionPContext(DbContextOptions<GestionPContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido);

            entity.ToTable("pedidos");

            entity.HasIndex(e => e.IdProducto, "IX_pedidos_idProducto");

            entity.HasIndex(e => e.Usuario, "IX_pedidos_usuario");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Nota).HasColumnName("nota");
            entity.Property(e => e.NumeroMesa).HasColumnName("numeroMesa");
            entity.Property(e => e.Usuario).HasColumnName("usuario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Pedidos).HasForeignKey(d => d.IdProducto);

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Pedidos).HasForeignKey(d => d.Usuario);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.ToTable("productos");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Aditivos).HasColumnName("aditivos");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Ingredientes)
                .HasMaxLength(50)
                .HasColumnName("ingredientes");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Usuario1);

            entity.ToTable("usuarios");

            entity.Property(e => e.Usuario1).HasColumnName("usuario");
            entity.Property(e => e.ApellidoM).HasColumnName("apellidoM");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(20)
                .HasColumnName("apellidoP");
            entity.Property(e => e.Cargo).HasColumnName("cargo");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(15)
                .HasColumnName("contraseña");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo).HasColumnName("sexo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    public String Conexion { get; }
    public GestionPContext(string valor)
    {
        Conexion = valor;
    }
}
