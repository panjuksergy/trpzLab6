using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SparkSwim.Core.Data;
using SparkSwim.Core.Models;
using IdentityService.Configuration;
using IdentityService.Interfaces;
using IdentityService.Services;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);

var app = builder.Build();
Configure(app);

app.Run();

void RegisterServices(IServiceCollection services)
{
    services.AddControllers();

    services.AddDbContext<UsersDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("UsersDbConnection"));
    });

    services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultTokenProviders();

    services.AddTransient<IJwtService, JwtService>();
    services.AddTransient<IUserIdentityRepository, UserIdentityRepository>();

    services.Configure<JwtConfiguration>(builder.Configuration.GetSection("JwtConfiguration"));
}

void Configure(IApplicationBuilder app)
{
    app.UseRouting();

    app.UseCors(_ => { _.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod(); });

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}