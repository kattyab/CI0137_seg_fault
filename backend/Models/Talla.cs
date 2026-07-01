using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("TALLAS")]
public partial class Talla
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    [StringLength(20)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdTallaNavigation")]
    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
