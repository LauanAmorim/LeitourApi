using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models;

[Table("tbl_publicacao")]
public class Post : BaseModel
{

    [Key]
    [Column("pk_publicacao_id",TypeName = "int")]
    public required int Id { get; set; }

    [Column("fk_usuario_id",TypeName = "int")]
    public required int UserId { get; set; }

    [Column("usuario_nome")]
    public string? UserName { get; }
    [Column("usuario_foto")]
    public string? UserPhoto { get; }

    [Column("publicacao_texto",TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

    [Column("likes")]
    public int Likes { get; }

    [Column("liked")]
    public bool Liked { get; }


    [Column("num_comentario")]
    public int Comment_Number { get; }
    
    [Column("data_alteracao", TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }
}