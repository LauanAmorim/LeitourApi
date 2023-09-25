using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeitourApi.Models;

public class Post
{
    [Key]
    public required int PostId { get; set; }

    public required int UserId { get; set; }

    public required string MessagePost { get; set; } = null!;

    public int Likes { get; set; }

    // public string image { get; set; }

    public required DateTime PostDate { get; set; }

    public DateTime? AlteratedDate { get; set; }

     public User? User { get; set; }
}
