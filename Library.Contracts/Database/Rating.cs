using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Contracts.Database;

public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(Book))]
    [Column("book_id")]
    public int BookID { get; set; }

    [Column("score")]
    public int Score { get; set; }
} 