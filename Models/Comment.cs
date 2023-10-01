

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models;

[Table("tbl_comentario")]
public class Comment
{
    [Key]
    [Column("pk_comentario_id", TypeName = "int")]
    public int CommentId { get; set; }

    [Column("fk_usuario_id", TypeName = "int")]
    public required int UserId { get; set; }

    [Column("fk_publicacao_id", TypeName = "int")]
    public required int PostId { get; set; }

    [Column("comentario_texto", TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

    [Column("comentario_data_criacao", TypeName = "date")]
    public required DateTime PostDate { get; set; }

    [Column("comentario_data_alteracao", TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }

}