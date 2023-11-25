﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models;

[Table("tbl_publicacao")]
public class Post
{

    [Key]
    [Column("pk_publicacao_id",TypeName = "int")]
    public required int Id { get; set; }

    [Column("fk_usuario_id",TypeName = "int")]
    public required int UserId { get; set; }

    [Column("usuario_nome")]
    public string? UserName { get; }

    [Column("publicacao_texto",TypeName = "varchar(250)")]
    public required string MessagePost { get; set; } = null!;

    [Column("likes")]
    public int Likes { get; }


    [Column("num_comentario")]
    public int Comment_Number { get; }

    [Column("publicacao_data_criacao",TypeName = "date")]
    public required DateTime PostDate { get; set; }
    
    [Column("publicacao_data_alteracao",TypeName = "date")]
    public DateTime? AlteratedDate { get; set; }

    

}