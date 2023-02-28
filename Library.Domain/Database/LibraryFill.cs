using Library.Contracts.Database;
using Library.Domain.Database;

public class LibraryRepository : ILibraryRepository
    {
        public LibraryRepository()
        {
            using (var context = new LibraryDbContext())
            {
                var books = new List<Book>
                {
                new Book
                {
                    Title = "a",
                    Cover = "b",
                    Content = "qwdqwf",
                    Author = "a1",
                    Genre = "b1"
                },
                new Book
                {
                    Title = "b",
                    Cover = "c",
                    Content = "ty-jkwn",
                    Author = "a2",
                    Genre = "b2"
                }
                };
                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }

    List<Book> ILibraryRepository.GetBooks()
    {
        using (var context = new LibraryDbContext())
        {
            var list = context.Books
                .ToList();
            return list;
        } 
    }
}