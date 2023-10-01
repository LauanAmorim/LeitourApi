using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LeitourApi.Models;

[Obsolete("Extra")]
[Table("tbFollowingPage")]
[PrimaryKey("Id","PageId")]
public class FollowingPage
{
    [Key]
    public int Id { get; set; }
    [Key]
    public int PageId { get; set; }

    public int RoleUser { get; set; }
    
    public FollowingPage() { }
    public FollowingPage(int Id, int pageId, int roleUser)
    {
        this.Id = Id;
        this.PageId = pageId;
        this.RoleUser = roleUser;
    }
}
