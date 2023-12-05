using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models;

[Table("tbl_publicacao")]
public class Post : BaseModel
{

    [Key]
    [Column("pk_publicacao_id", TypeName = "int")]
    public required int Id { get; set; }

    [Column("fk_usuario_id", TypeName = "int")]
    public required int UserId { get; set; }

  //  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("usuario_nome")]
    public string? UserName { get; }

   // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("usuario_email")]
    public string? Email { get;  }

   // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("usuario_foto")]
    public string? UserPhoto { get;}

    [Column("publicacao_texto", TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

   // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("likes")]
    public int Likes { get; }

   // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("liked")]
    public bool Liked { get; } = false;
    
   // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column("num_comentario")]
    public int Comment_Number { get; }

    [Column("data_alteracao", TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }
}