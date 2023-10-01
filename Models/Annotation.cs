using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeitourApi.Models;

[Table("tbl_anotacao")]
public class Annotation
{
    [Key]
    [Column("pk_anotacao_id")]
    public int AnnotationId { get; set; }
    [Column("fk_livro_salvo_id")]
    public int SavedBookId { get; set; }
    [Column("anotacao_texto")]
    public string AnnotationText { get; set; } = null!;
    
    [Column("anotacao_data_criacao")]
    public DateTime CreatedDate { get; set; }
    [Column("anotacao_data_alteracao")]
    public DateTime AlteratedDate { get; set; }


}
