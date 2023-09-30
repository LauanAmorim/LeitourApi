using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeitourApi.Models;

[Table("tbUsuario")]
public class User
{
    [Key]
    [Column("Id",TypeName = "int")]
    [Required]
    public int Id { get; set; }

    [Column("Nome",TypeName = "varchar(30)")]
    [Required]
    public required string NameUser { get; set;}

    [Column("Email",TypeName = "varchar(100)")]
    [Required]
    public required string Email { get; set; } = null!;

    [Column("Senha",TypeName = "varchar(64)")]
    [Required]
    public required string Password { get; set; } = null!;

    [Column("FotoPerfil",TypeName = "varchar(100)")]
    public string? ProfilePhoto { get; set; }

    [Column("Acesso", TypeName="Enum")]
    public string Acess {get; set;} = null!;
}