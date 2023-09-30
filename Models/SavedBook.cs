
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models
{
    [Table("tbBook")]
    public class SavedBook
    {

        [Key]
        public int Id { get; set; }
        [Column("IdUsuario")]
        public int UserId { get; set; }
        [Column("publico")]
        public bool Public { get; set; }
        public string BookKey { get; set; } = null!;
    }
}
