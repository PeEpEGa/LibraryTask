using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Contracts.Database;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(300)]
    public string Title { get; set; }

    [Required]
    [Column("cover")]
    [MaxLength(100)]
    public string Cover { get; set; }

    [Column("content")]
    [MaxLength(2000)]
    public string Content { get; set; }

    [Required]
    [Column("author")]
    [MaxLength(250)]
    public string Author { get; set; }

    [Required]
    [Column("genre")]
    [MaxLength(100)]
    public string Genre { get; set; }
}