using Library.Contracts.Http;
using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Queries;

public class LibraryQueryBookInfo : IRequest<LibraryQueryBookInfoResult>
{
    public int BookInfoId { get; set; }
}

public class LibraryQueryBookInfoResult
{
    public BookInfoDTO Book { get; set; }
}

internal class LibraryQueryBookInfoHandler : BaseHandler<LibraryQueryBookInfo, LibraryQueryBookInfoResult>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryQueryBookInfoHandler(LibraryDbContext dbContext, ILogger<LibraryQueryBookInfoHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<LibraryQueryBookInfoResult> HandleInternal(LibraryQueryBookInfo request, CancellationToken cancellationToken)
    {
        var book = _dbContext.Books.SingleOrDefault(n => n.Id == request.BookInfoId);
        try
        {
            return book == null ? 
            throw new ArgumentNullException("No wishlist with such id") 
            : new LibraryQueryBookInfoResult 
            {
                Book = new BookInfoDTO
                {
                    Id = book.Id,
                    Author = book.Author,
                    Title = book.Title,
                    Cover = book.Cover,
                    Content = book.Content,
                    Rating = _dbContext.Ratings.Where(n => book.Id == n.BookID).Select(x => x.Score).Count() > 0 ? _dbContext.Ratings.Where(n => book.Id == n.BookID).Select(x => x.Score).Sum() / 
                              (decimal)(_dbContext.Ratings.Where(x => book.Id ==x.BookID).Count()) : 0,
                    Reviews = _dbContext.Reviews.Where(n => book.Id == n.BookID).ToList()
                }
            };
        }
        catch 
        {
            throw new ArgumentException("Some problems");
        }
        
    }
}