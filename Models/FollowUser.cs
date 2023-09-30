using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models;

[Table("tbFollowingList")]
[PrimaryKey(nameof(Id), nameof(FollowingEmail))]
public partial class FollowUser
{
    [Key]
    public int Id { get; set; }

    [Key]
    public string FollowingEmail { get; set; }

    public FollowUser(){}
    public FollowUser(int Id,string email){
        this.Id = Id;
        this.FollowingEmail = email;
    }
}
