using System.ComponentModel.DataAnnotations;

namespace LeitourApi.Models
{
   
    public partial class BookApi
    {
        [Key]
        public string Key { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Authors { get; set; }
        public string? Publisher { get; set; }
        public string? PublishedDate { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public int Pages { get; set; }
        public string? ISBN_10 { get; set; }
        public string? ISBN_13 { get; set; }
        public string? Language { get; set; }
        public string? Cover { get; set; }
    }
}