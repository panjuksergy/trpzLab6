using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SparkSwim.GoodsService;
using SparkSwim.GoodsService.Goods.Models;
using SparkSwim.GoodsService.Interfaces;

namespace CinemaAdminTests;

public class CinemaWebAppFactory : WebApplicationFactory<Program>
{
    private string storName = "Guid.NewGuid().ToString()";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(ICinemaDbContext));

            services.AddDbContext<ICinemaDbContext, CinemaDbContext>(options =>
            {
                options.UseInMemoryDatabase(storName);
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<CinemaDbContext>();

                // Clear existing data
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SeedDbContext(context);
            }
        });   
    }

    private void SeedDbContext(CinemaDbContext context)
    {
        var movieId = Guid.NewGuid();
        var cinemaProdId = Guid.NewGuid();

        var movie = new Movie
        {
            Id = movieId,
            Title = "testtile",
            Year = 2023,
            Director = "Paniuk Serhii",
            IsDeleted = false,
            Genre = Genre.A,
            CreationDate = DateTime.Now,
        };
        
        context.CinemaProds.Add(new CinemaProd
        {
            Id = cinemaProdId,
            Movie = movie,
            DateStart = DateTime.MinValue,
            DateFinish = DateTime.Now,
            AssignCinema = "test",
            IsDeleted = false,
        });

        context.Movies.Add(movie);

        context.SaveChanges();
    }
}