

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models;

[Table("tbl_comentario")]
public class Comment : BaseModel
{
    [Key]
    [Column("pk_comentario_id", TypeName = "int")]
    public int CommentId { get; set; }

    [Column("fk_usuario_id", TypeName = "int")]
    public required int UserId { get; set; }

    [Column("usuario_nome")]
    public string? UserName { get; set; }
    [Column("usuario_foto")]
    public string? UserPhoto { get; }

    [Column("fk_publicacao_id", TypeName = "int")]
    public required int PostId { get; set; }

    [Column("comentario_texto", TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

    [Column("data_alteracao", TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }

}