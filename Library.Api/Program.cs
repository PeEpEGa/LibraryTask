using System.Reflection;
using Library.Contracts.Database;
using Library.Domain;
using Library.Domain.Database;
using Library.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(builder.Configuration);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();
builder.Services.AddDomainServices();

static void AddCustomerData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<LibraryDbContext>();

    List<Book> bookList = new List<Book>();
    for (int i = 0; i < 10; i++)
    {
        var book = new Book
        {
            Title = Guid.NewGuid().ToString(),
            Cover = Guid.NewGuid().ToString(),
            Content = Guid.NewGuid().ToString(),
            Author = Guid.NewGuid().ToString(),
            Genre = Guid.NewGuid().ToString()
        };
        bookList.Add(book);
    }

    Random r = new Random();
    List<Rating> ratingList = new List<Rating>();
    List<Review> reviewList = new List<Review>();

    for (int i = 0; i < 150; i++)
    {
        var rating = new Rating
        {
            BookID = r.Next(1,bookList.Count() + 1),
            Score = r.Next(1,6)
        };
        ratingList.Add(rating);
    }

    for (int i = 0; i < 150; i++)
    {
        var review = new Review
        {
            BookID = r.Next(1,bookList.Count() + 1),
            Message = $"message{i}",
            Reviewer = $"reviewer{i}"
        };
        reviewList.Add(review);
    }

    db.Books.AddRange(bookList);
    db.Ratings.AddRange(ratingList);
    db.Reviews.AddRange(reviewList);

    db.SaveChanges();
}

var app = builder.Build();
AddCustomerData(app);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
