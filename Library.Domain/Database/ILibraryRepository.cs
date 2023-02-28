using Library.Contracts.Database;

public interface ILibraryRepository
{
    public List<Book> GetBooks();
}