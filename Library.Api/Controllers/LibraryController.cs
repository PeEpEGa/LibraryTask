using Library.Api.Controllers;
using Library.Contracts.Database;
using Library.Contracts.Http;
using Library.Domain.Commands;
using Library.Domain.Database;
using Library.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class LibraryController : BaseController
{
    private readonly IMediator _mediator;
    private readonly LibraryDbContext _libraryDbContext;

    public LibraryController(IMediator mediator, LibraryDbContext libraryDbContext)
    {
        _mediator = mediator;
        _libraryDbContext = libraryDbContext;
    }


    [HttpGet("GetAllBooks")]
    public Task<IActionResult> GetBooks(string ord, CancellationToken cancellationToken)
    {
        //[FromRoute] 
        return SafeExecute(async () => 
        {
            var query = new LibraryQuery
            {
                OrderedBy = ord
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new GetBooksResponse
            {
                Books = result.Books
            };

            return Ok(response);

        }, cancellationToken);
    }

    [HttpGet("GetTop10Books")]
    public Task<IActionResult> GetTop10Books(string genre, CancellationToken cancellationToken)
    {
        //[FromRoute] 
        return SafeExecute(async () => 
        {
            var query = new LibraryQueryTop10
            {
                Genre = genre
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new GetBooksResponse
            {
                Books = result.Top10Books
            };

            return Ok(response);

        }, cancellationToken);
    }

    [HttpGet("GetBookInfo")]
    public Task<IActionResult> GetBookInfo(int id, CancellationToken cancellationToken)
    {
        return SafeExecute(async () => 
        {
            var query = new LibraryQueryBookInfo
            {
                BookInfoId = id
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new GetBookResponse
            {
                Book = result.Book
            };

            return Ok(response);

        }, cancellationToken);
    }

    [HttpDelete("DeleteBook")]
    public Task<IActionResult> DeleteBook(int id, string secretKey, CancellationToken cancellationToken)
    {
        return SafeExecute(async () => 
        {
            var query = new LibraryCommandDeleteBook
            {
                BookId = id,
                SecretKey = secretKey
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new BookResponse
            {
                Response = result.Deleted == true ? $"Book with id: {id} was deleted." : "Book hasn't found."
            };

            return Ok(response);

        }, cancellationToken);
    }


    [HttpPost("CreateBook")]
    public Task<IActionResult> CreateBook([FromBody]Book book, CancellationToken cancellationToken)
    {
        return SafeExecute(async () => 
        {
            var query = new LibraryCommandCreateBook
            {
                BookId = book.Id,
                Tilte = book.Title,
                Cover = book.Cover,
                Content = book.Content,
                Genre = book.Genre,
                Author = book.Author
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new BookResponse
            {
                Response = result.Created == true ? $"Book with id: {result.Id} was created/updated." : "Book hasn't found."
            };

            return Created("", response);

        }, cancellationToken);
    }

    [HttpPost("SaveAReview")]
    public Task<IActionResult> SaveAReview(int id, string message, string reviewer, CancellationToken cancellationToken)
    {
        return SafeExecute(async () => 
        {
            var query = new LibraryCommandSaveAReview
            {
                BookId = id,
                Message = message,
                Reviewer = reviewer
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new BookResponse
            {
                Response = result.Added == true ? $"Review with book id: {result.Id} was added." : "Book hasn't found."
            };

            return Created("", response);

        }, cancellationToken);
    }

    [HttpPost("RateABook")]
    public Task<IActionResult> RateABook(int id, int score, CancellationToken cancellationToken)
    {
        return SafeExecute(async () => 
        {
            var query = new LibraryCommandRateABook
            {
                BookId = id,
                Score = score
            };

            var result = await _mediator.Send(query, cancellationToken);
        
            var response  = new BookResponse
            {
                Response = result.Added == true ? $"Score {result.AddedScore} was added." : "Book hasn't found."
            };

            return Created("", response);

        }, cancellationToken);
    }
}
