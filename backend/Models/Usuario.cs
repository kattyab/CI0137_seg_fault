using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Backend.Api.Models;

[Table("USUARIOS")]
[Index("Email", Name = "uq_usu_email", IsUnique = true)]
public partial class Usuario
{
    [Key]
    [Column("id")]
    [StringLength(30)]
    public string Id { get; set; } = null!;

    [Column("nombre")]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Column("telefono")]
    [StringLength(20)]
    public string Telefono { get; set; } = null!;

    [Column("passwordHash")]
    [StringLength(100)]
    public string PasswordHash { get; set; } = null!;

    [Column("passwordSalt")]
    [StringLength(100)]
    public string PasswordSalt { get; set; } = null!;

    [Column("createdDate")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Ordene> Ordenes { get; set; } = new List<Ordene>();
}
