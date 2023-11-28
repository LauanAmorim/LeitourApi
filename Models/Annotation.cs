using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeitourApi.Models;

[Table("tbl_anotacao")]
public class Annotation : BaseModel
{
    [Key]
    [Column("pk_anotacao_id")]
    public int AnnotationId { get; set; }
    [Column("fk_livro_salvo_id")]
    public int SavedBookId { get; set; }
    [Column("anotacao_texto")]
    public string AnnotationText { get; set; } = null!;
    [Column("data_alteracao", TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }


}
