using Library.Contracts.Database;
using Library.Contracts.Http;
using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Queries;

public class LibraryQuery : IRequest<LibraryQueryResult>
{
    public string OrderedBy { get; set; }
}
public class LibraryQueryResult
{
    public List<BookDTO> Books { get; set; }
}
internal class LibraryQueryHandler : BaseHandler<LibraryQuery, LibraryQueryResult>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryQueryHandler(LibraryDbContext dbContext, ILogger<LibraryQueryHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }
    protected override async Task<LibraryQueryResult> HandleInternal(LibraryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var books = _dbContext.Books.Select(n => new BookDTO()
            {
                Id = n.Id,
                Title = n.Title,
                Author = n.Author,
                Rating = _dbContext.Ratings.Where(x => x.BookID == n.Id).Count() > 0 ? (decimal)(_dbContext.Ratings.Where(x => x.BookID == n.Id).Select(n => n.Score).Sum()) / 
                         (decimal)(_dbContext.Ratings.Where(x => x.BookID == n.Id).Count()) : 0,
                ReviewsNumber = _dbContext.Reviews.Where(x => x.BookID == n.Id).Count()
            }).ToList();

            if(request.OrderedBy != null && (request.OrderedBy.ToLower() == "title" || request.OrderedBy.ToLower() == "author"))
            {
                books = request.OrderedBy.ToLower() == "title" ?  books.OrderBy(n => n.Title).ToList() : books.OrderBy(n => n.Author).ToList();
            }
        
            return new LibraryQueryResult
            {
                Books = books
            };
        }
        catch 
        {
            throw new ArgumentException("Some problems");
        }
    }
}