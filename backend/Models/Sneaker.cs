using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("SNEAKERS")]
public partial class Sneaker
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_categoria")]
    public int IdCategoria { get; set; }

    [Column("nombre")]
    [StringLength(150)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("precio", TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [Column("descripcion")]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [ForeignKey("IdCategoria")]
    [InverseProperty("Sneakers")]
    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    [InverseProperty("IdSneakerNavigation")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
