using Library.Domain.Database;
using Library.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Domain;

public static class LibraryDomainExtentions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddDbContext<LibraryDbContext>(o => o.UseInMemoryDatabase("LibraryDb"));
        services.AddMediatR(o => o.RegisterServicesFromAssemblies(typeof(LibraryQuery).Assembly));
        return services;
    }
}