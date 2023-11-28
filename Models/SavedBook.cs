
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models
{
    [Table("tbl_livro_salvo")]
    public class SavedBook : BaseModel
    {

        [Key]
        [Column("pk_livro_salvo_id")]
        public int Id { get; set; }

        [Column("fk_usuario_id")]
        public int UserId { get; set; }

        [Column("livro_salvo_publico")]
        public bool Public { get; set; }

        [Column("livro_salvo_chave_livro")]
        public string BookKey { get; set; } = null!;

        [Column("livro_salvo_capa")]
        public string BookCover { get; set; } = null!;

        [Column("livro_salvo_titulo")]
        public string BookTitle { get; set; } = null!;
    
    }
}
