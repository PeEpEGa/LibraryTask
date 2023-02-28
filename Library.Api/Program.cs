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


//db
builder.Services.AddHealthChecks();
// builder.Services.AddDbContext<LibraryDbContext>(o => o.UseInMemoryDatabase("LibraryDb"));
// builder.Services.AddMediatR(o => o.RegisterServicesFromAssemblies(typeof(LibraryQuery).Assembly));
builder.Services.AddDomainServices();

static void AddCustomerData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<LibraryDbContext>();

    //modelBuilder.Entity<Blog>().HasData(new Blog { BlogId = 1, Url = "http://sample.com" });

    var book1 = new Book
    {
        Title = "dgkh",
        Cover = "b",
        Content = "tergrg",
        Author = "g1",
        Genre = "b1"
    };
    var book2 = new Book
    {
        Title = "olqw",
        Cover = "c",
        Content = "ty-jkwn",
        Author = "o2",
        Genre = "b2"
    };
    var book3 = new Book
    {
        Title = "akviw",
        Cover = "d",
        Content = "mrqr34",
        Author = "a3",
        Genre = "b3"
    };

    Random r = new Random();
    List<Rating> ratingList = new List<Rating>();
    List<Review> reviewList = new List<Review>();

    for (int i = 0; i < 50; i++)
    {
        var rating = new Rating
        {
            BookID = r.Next(1,4),
            Score = r.Next(1,6)
        };
        ratingList.Add(rating);
    }

    for (int i = 0; i < 50; i++)
    {
        var review = new Review
        {
            BookID = r.Next(1,4),
            Message = $"message{i}",
            Reviewer = $"reviewer{i}"
        };
        reviewList.Add(review);
    }


    db.Books.Add(book1);
    db.Books.Add(book2);
    db.Books.Add(book3);

    db.Ratings.AddRange(ratingList);
    db.Reviews.AddRange(reviewList);

    db.SaveChanges();
}

var app = builder.Build();
//temp
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
