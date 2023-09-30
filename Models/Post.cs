using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models;

[Table("tbPublicacoes")]
public class Post
{
    [Key]
    [Column("IdPublicacao",TypeName = "int")]
    public required int Id { get; set; }
    [Column("IdUsuario",TypeName = "int")]
    public required int UserId { get; set; }

    [Column("Conteudo",TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

    public required int Likes { get; set; }

    // public string image { get; set; }

    [Column("DataCriacao",TypeName = "date")]
    public required DateTime PostDate { get; set; }
    
    [Column("DataAlteracao",TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }
}