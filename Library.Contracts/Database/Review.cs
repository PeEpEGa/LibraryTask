using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Contracts.Database;

[Table("tbl_reviews", Schema = "public")]
public class Review 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("message")]
    [MaxLength(500)]
    public string Message { get; set; }

    // [ForeignKey(nameof(Book))]
    public int BookID { get; set; }

    [Required]
    [Column("reviewer")]
    [MaxLength(250)]
    public string Reviewer { get; set; }
}