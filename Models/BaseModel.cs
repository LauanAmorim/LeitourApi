using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LeitourApi.Models;


public class BaseModel 
{
    public DateTime CreatedDate { get; set; }
    public DateTime? AlteratedDate { get; set; }
}