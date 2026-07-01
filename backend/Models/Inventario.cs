using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("INVENTARIO")]
[Index("IdSneaker", "IdColor", "IdTalla", Name = "uq_inventario", IsUnique = true)]
public partial class Inventario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_sneaker")]
    public int IdSneaker { get; set; }

    [Column("id_color")]
    public int? IdColor { get; set; }

    [Column("id_talla")]
    public int IdTalla { get; set; }

    [Column("stock")]
    public int Stock { get; set; }

    [Column("imagen_url")]
    [StringLength(255)]
    [Unicode(false)]
    public string? ImagenUrl { get; set; }

    [InverseProperty("IdInventarioNavigation")]
    public virtual ICollection<DetalleOrden> DetalleOrdens { get; set; } = new List<DetalleOrden>();

    [ForeignKey("IdColor")]
    [InverseProperty("Inventarios")]
    public virtual Colore? IdColorNavigation { get; set; }

    [ForeignKey("IdSneaker")]
    [InverseProperty("Inventarios")]
    public virtual Sneaker IdSneakerNavigation { get; set; } = null!;

    [ForeignKey("IdTalla")]
    [InverseProperty("Inventarios")]
    public virtual Talla IdTallaNavigation { get; set; } = null!;
}
