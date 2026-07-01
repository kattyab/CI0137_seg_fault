using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("TRANSACCIONES_PAGO")]
public partial class TransaccionesPago
{
    [Key]
    [Column("id")]
    [StringLength(30)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [Column("id_orden")]
    [StringLength(30)]
    [Unicode(false)]
    public string IdOrden { get; set; } = null!;

    [Column("nombre_tarjeta")]
    [StringLength(100)]
    [Unicode(false)]
    public string NombreTarjeta { get; set; } = null!;

    [Column("bin_tarjeta")]
    [StringLength(6)]
    [Unicode(false)]
    public string BinTarjeta { get; set; } = null!;

    [Column("ultimos4")]
    [StringLength(4)]
    [Unicode(false)]
    public string Ultimos4 { get; set; } = null!;

    [Column("marca")]
    [StringLength(30)]
    [Unicode(false)]
    public string Marca { get; set; } = null!;

    [Column("exp_mes")]
    public int ExpMes { get; set; }

    [Column("exp_anio")]
    public int ExpAnio { get; set; }

    [Column("monto", TypeName = "decimal(10, 2)")]
    public decimal Monto { get; set; }

    [Column("estado")]
    [StringLength(30)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [Column("codigo_autorizacion")]
    [StringLength(30)]
    [Unicode(false)]
    public string? CodigoAutorizacion { get; set; }

    [Column("motivo_rechazo")]
    [StringLength(150)]
    [Unicode(false)]
    public string? MotivoRechazo { get; set; }

    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdOrden")]
    [InverseProperty("TransaccionesPagos")]
    public virtual Ordene IdOrdenNavigation { get; set; } = null!;
}
