

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models;

[Table("tbComentarios")]
public class Comment 
{
       [Key]
       [Column("Id",TypeName = "int")]
       public int CommentId {get; set;}

       [Column("IdPublicacao",TypeName = "int")]
    public required int Id { get; set; }
    [Column("IdUsuario",TypeName = "int")]
    public required int UserId { get; set; }

    [Column("ConteudoComentario",TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;


    [Column("DataCriacao",TypeName = "date")]
    public required DateTime PostDate { get; set; }
    
    [Column("DataAlteracao",TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }

}