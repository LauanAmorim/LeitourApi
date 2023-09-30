using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeitourApi.Models;

[Table("tbAnnotation")]
public class Annotation
{
    [Key]
    [Column("IddAnnotation")]
    public int AnnotationId { get; set; }
    [Column("IdBook")]
    public int SavedBookId { get; set; }
    [Column("ConteudoAnotacao")]
    public string AnnotationText { get; set; } = null!;
    
    [Column("DataAnotacao")]
    public DateTime CreatedDate { get; set; }
    [Column("DataAlteracao")]
    public DateTime AlteratedDate { get; set; }


}
