using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("DETALLE_ORDEN")]
public partial class DetalleOrden
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_orden")]
    [StringLength(30)]
    public string IdOrden { get; set; } = null!;

    [Column("id_inventario")]
    public int IdInventario { get; set; }

    [Column("cantidad")]
    public int Cantidad { get; set; }

    [Column("precio_unit", TypeName = "decimal(10, 2)")]
    public decimal PrecioUnit { get; set; }

    [ForeignKey("IdInventario")]
    [InverseProperty("DetalleOrdens")]
    public virtual Inventario IdInventarioNavigation { get; set; } = null!;

    [ForeignKey("IdOrden")]
    [InverseProperty("DetalleOrdens")]
    public virtual Ordene IdOrdenNavigation { get; set; } = null!;
}
