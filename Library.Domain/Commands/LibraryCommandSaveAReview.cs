using Library.Contracts.Database;
using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Commands;

public class LibraryCommandSaveAReview : IRequest<LibraryCommandSaveAReviewResult>
{
    public string Message { get; set; }
    public string Reviewer { get; set; }
    public int BookId { get; set; }
}

public class LibraryCommandSaveAReviewResult
{
    public bool Added { get; set; }
    public int Id { get; set; }
}

internal class LibraryCommandSaveAReviewHandler : BaseHandler<LibraryCommandSaveAReview, LibraryCommandSaveAReviewResult>
{
    private readonly LibraryDbContext _dbContext;
    public LibraryCommandSaveAReviewHandler(LibraryDbContext dbContext, ILogger<LibraryCommandSaveAReviewHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
    }

    protected override async Task<LibraryCommandSaveAReviewResult> HandleInternal(LibraryCommandSaveAReview request, CancellationToken cancellationToken)
    {
        var added = false;
        var book = _dbContext.Books.SingleOrDefault(n => n.Id == request.BookId);
        if(book != null)
        {
            added = true;
            await _dbContext.Reviews.AddAsync(new Review
            {
                BookID = request.BookId,
                Message = request.Message,
                Reviewer = request.Reviewer
            });
            await _dbContext.SaveChangesAsync();
        }
        return new LibraryCommandSaveAReviewResult
        {
            Added = added,
            Id = request.BookId
        };
    }
}