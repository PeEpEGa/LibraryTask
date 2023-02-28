using Library.Contracts.Http;
using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Queries;

public class LibraryQueryTop10 : IRequest<LibraryQueryTop10Result>
{
    public string Genre { get; set; }
}

public class LibraryQueryTop10Result
{
    public List<BookDTO> Top10Books { get; set; }
}

internal class LibraryQueryTop10Handler : BaseHandler<LibraryQueryTop10, LibraryQueryTop10Result>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryQueryTop10Handler(LibraryDbContext dbContext, ILogger<LibraryQueryTop10Handler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<LibraryQueryTop10Result> HandleInternal(LibraryQueryTop10 request, CancellationToken cancellationToken)
    {
        try
        {
            var top10Books = _dbContext.Books.Where(n => request.Genre == null ? n.Genre == n.Genre: n.Genre == request.Genre).Select(n => new BookDTO()
            {
                Id = n.Id,
                Title = n.Title,
                Author = n.Author,
                Rating = _dbContext.Ratings.Where(x => x.BookID == n.Id).Select(n => n.Score).Count() > 0 ? (decimal)(_dbContext.Ratings.Where(x => x.BookID == n.Id).Select(n => n.Score).Sum()) / 
                         (decimal)(_dbContext.Ratings.Where(x => x.BookID == n.Id).Count()) : 0,
                ReviewsNumber = _dbContext.Reviews.Where(x => x.BookID == n.Id).Count()
            }).Where(n => n.Rating > 3 && n.ReviewsNumber > 10).Take(10).OrderBy(n => n.Rating).ToList();

            return new LibraryQueryTop10Result
            {
                Top10Books = top10Books
            };
        }
        catch 
        {
            throw new ArgumentException("Some problems");
        }
    }
}