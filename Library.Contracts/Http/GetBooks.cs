namespace Library.Contracts.Http;

public class GetBooksResponse
{
    public List<BookDTO> Books { get; init; }
}

public class GetBookResponse
{
    public BookInfoDTO Book { get; init; }
}


public class BookResponse
{
    public string Response { get; init; }
}
