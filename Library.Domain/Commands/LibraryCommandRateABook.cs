using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Commands;

public class LibraryCommandRateABook : IRequest<LibraryCommandRateABookResult>
{
    public int BookId { get; set; }
    public int Score { get; set; }
}

public class LibraryCommandRateABookResult
{
    public bool Added { get; set; }
    public int AddedScore { get; set; }
}

internal class LibraryCommandRateABookHandler : BaseHandler<LibraryCommandRateABook, LibraryCommandRateABookResult>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryCommandRateABookHandler(LibraryDbContext dbContext, ILogger<LibraryCommandRateABookHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }
    protected override async Task<LibraryCommandRateABookResult> HandleInternal(LibraryCommandRateABook request, CancellationToken cancellationToken)
    {
        var added = false;
        var book = _dbContext.Books.SingleOrDefault(n => n.Id == request.BookId);
        if(book != null && request.Score > 0 && request.Score < 6)
        {
            added = true;
            await _dbContext.Ratings.AddAsync(new Contracts.Database.Rating {
                BookID = request.BookId,
                Score = request.Score
            });
            await _dbContext.SaveChangesAsync();
        }
        return new LibraryCommandRateABookResult
        {
            Added = added,
            AddedScore = request.Score
        };
    }
}