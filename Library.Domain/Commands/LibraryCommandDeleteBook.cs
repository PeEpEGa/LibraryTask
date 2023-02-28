using Library.Domain.Base;
using Library.Domain.Database;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Library.Domain.Commands;

public class LibraryCommandDeleteBook : IRequest<LibraryCommandDeleteBookResult>
{
    public int BookId { get; set; }
    public string SecretKey { get; set; }
}

public class LibraryCommandDeleteBookResult
{
    public bool Deleted { get; set; }
}

internal class LibraryCommandDeleteBookHandler : BaseHandler<LibraryCommandDeleteBook, LibraryCommandDeleteBookResult>
{
    private readonly LibraryDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public LibraryCommandDeleteBookHandler(LibraryDbContext dbContext, IConfiguration configuration, ILogger<LibraryCommandDeleteBookHandler> logger) : base(logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    protected override async Task<LibraryCommandDeleteBookResult> HandleInternal(LibraryCommandDeleteBook request, CancellationToken cancellationToken)
    {
        var bookToDelete = _dbContext.Books.SingleOrDefault(n => n.Id == request.BookId);
        var deleted = false;

        if(request.SecretKey == _configuration.GetSection("AppSettings:SecretKey").Value && bookToDelete != null)
        {
            deleted = true;
            var reviewsToDelete = _dbContext.Reviews.Where(n => n.BookID == request.BookId);
            var ratingsToDelete = _dbContext.Ratings.Where(n => n.BookID == request.BookId);
            _dbContext.Books.Remove(bookToDelete);
            _dbContext.Reviews.RemoveRange(reviewsToDelete);
            _dbContext.Ratings.RemoveRange(ratingsToDelete);
            await _dbContext.SaveChangesAsync();
        }
        return new LibraryCommandDeleteBookResult
        {
            Deleted = deleted
        };
    }
}