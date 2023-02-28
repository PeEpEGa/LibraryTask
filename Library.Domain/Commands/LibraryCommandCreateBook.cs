using Library.Contracts.Database;
using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Commands;

public class LibraryCommandCreateBook : IRequest<LibraryCommandCreateBookResult>
{
    public int BookId { get; set; }
    public string Tilte { get; set; }
    public string Cover { get; set; }
    public string Content { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
}

public class LibraryCommandCreateBookResult
{
    public bool Created { get; set; }
    public int Id { get; set; }
}

internal class LibraryCommandCreateBookHandler : BaseHandler<LibraryCommandCreateBook, LibraryCommandCreateBookResult>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryCommandCreateBookHandler(LibraryDbContext dbContext, ILogger<LibraryCommandCreateBookHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }
    protected override async Task<LibraryCommandCreateBookResult> HandleInternal(LibraryCommandCreateBook request, CancellationToken cancellationToken)
    {
        var created = false;
        var id = -1;
        if(request.BookId == 0)
        {
            var newBook = new Book
            {
                Title = request.Tilte,
                Cover = request.Cover,
                Content = request.Content,
                Genre = request.Genre,
                Author = request.Author
            };
            created = true;
            id = newBook.Id;
            await _dbContext.Books.AddAsync(newBook);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            var book = _dbContext.Books.AsNoTracking().FirstOrDefault(n => n.Id == request.BookId);
            if(book != null)
            {
                book = new Book
                {
                    Id = request.BookId,
                    Title = request.Tilte,
                    Cover = request.Cover,
                    Content = request.Content,
                    Genre = request.Genre,
                    Author = request.Author
                };
                created = true;
                id = book.Id;
                _dbContext.Books.Update(book);
                await _dbContext.SaveChangesAsync();
            }
        }
        return new LibraryCommandCreateBookResult
        {
            Created = created,
            Id = id
        };
    }
}