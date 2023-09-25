using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeitourApi.Models;

[Table("tbUser")]
public class User
{
    [Key]
    [Column("UserId",TypeName = "int")]
    [Required]
    public int UserId { get; set; }

    [Column("NameUser",TypeName = "varchar(20)")]
    [Required]
    public string NameUser { get; set;}

    [Column("Email",TypeName = "varchar(50)")]
    [Required]
    public required string Email { get; set; } = null!;

    [Column("Password",TypeName = "varchar(32)")]
    [Required]
    public required string Password { get; set; } = null!;

    [Column("ProfilePhoto",TypeName = "varchar(100)")]
  //  [Required]
    public string? ProfilePhoto { get; set; }

    public ICollection<Post> Posts { get; } = new List<Post>();
}