using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeitourApi.Models;


public class BaseModel 
{
    [Column("data_criacao", TypeName = "date")]
    public DateTime CreatedDate { get; set; }
}