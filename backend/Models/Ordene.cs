using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("ORDENES")]
public partial class Ordene
{
    [Key]
    [Column("id")]
    [StringLength(30)]
    public string Id { get; set; } = null!;

    [Column("id_usuario")]
    [StringLength(30)]
    public string IdUsuario { get; set; } = null!;

    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [Column("estado")]
    [StringLength(30)]
    public string Estado { get; set; } = null!;

    [Column("total", TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    [InverseProperty("IdOrdenNavigation")]
    public virtual ICollection<DetalleOrden> DetalleOrdens { get; set; } = new List<DetalleOrden>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Ordenes")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
