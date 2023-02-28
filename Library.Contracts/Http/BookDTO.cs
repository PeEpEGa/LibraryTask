using Library.Contracts.Database;

namespace Library.Contracts.Http;

public class BookDTO
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public decimal Rating { get; set; }
    public int ReviewsNumber { get; set; }
}


public class BookInfoDTO
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Author { get; init; }
    public string Cover { get; set; }
    public string Content { get; set; }
    public decimal Rating { get; set; }
    public List<Review> Reviews { get; set; }
}