using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using CinemaAdminTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Newtonsoft.Json;
using SparkSwim.GoodsService;
using SparkSwim.GoodsService.Goods.Models;
using SparkSwim.GoodsService.Interfaces;
using Xunit;

public class RepoTests
{
    private CinemaWebAppFactory _cinemaWebAppFactory;
    private HttpClient _httpClient;
    public RepoTests()
    {
        _cinemaWebAppFactory = new CinemaWebAppFactory();
        _httpClient = _cinemaWebAppFactory.CreateClient();
    }
    
    [Fact]
    public async Task GetAllMovies_ReturnsCorrectMoviesCount()
    {
        var response = await _httpClient.GetAsync("/api/cinema/GetAllMovies");
        
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var movieCount = Newtonsoft.Json.JsonConvert
            .DeserializeObject<List<Movie>>(jsonResponse).Count;
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, movieCount);
    }

    [Fact]
    public async Task GetAllProdForCinema_ReturnCorrectProdByCinema()
    {
        var cinemaName = "test";
        var response = await _httpClient.GetAsync($"/api/Cinema/GetAllProdForCinema?cinema={cinemaName}");
        
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var prodCount = Newtonsoft.Json.JsonConvert
            .DeserializeObject<List<CinemaProd>>(jsonResponse).Count;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, prodCount);
    }
    
    [Fact]
    public async Task CreateMovie_CheckIfCreatesAsExpect()
    {
        var testitle = "testTitle4444";

        var obj = new Movie
        {
            Id = Guid.NewGuid(),
            IsDeleted = false,
            Director = "TestDirector",
            Genre = Genre.C,
            CinemaProdId = null,
            CreationDate = DateTime.Now,
            Year = 4444,
            Title = testitle
        };
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj),  System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("api/Cinema/CreateMovie", content);
        
        var verifyCreation = await _httpClient.GetAsync("api/Cinema/GetAllMovies");
        
        var jsonResponse = await verifyCreation.Content.ReadAsStringAsync();

        var movies = Newtonsoft.Json.JsonConvert
            .DeserializeObject<List<Movie>>(jsonResponse);

        var isCreatedSuccess = !movies?.Where(_ => _.Title == testitle).IsNullOrEmpty();

        response.EnsureSuccessStatusCode();
        Assert.Equal(true, isCreatedSuccess);
    }
    
    [Fact]
    public async Task UpdateProdCinema()
    {
        var newCinemaProdCinema = "4422";
        using var scope = _cinemaWebAppFactory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ICinemaDbContext>();
        var res = await context.CinemaProds.FirstOrDefaultAsync();

        var prodId = res.Id;

        var cinemaProdUpdateDTO = new CinemaProdUpdateDTO
        {
            CinemaProd = res,
            NewAssignCinema = newCinemaProdCinema,
        };
        
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(cinemaProdUpdateDTO),  System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/Cinema/assignProdToAnotherCinema", content);

        
        using var newScope = _cinemaWebAppFactory.Services.CreateScope();
        var newContext = newScope.ServiceProvider.GetRequiredService<ICinemaDbContext>();
        var checkForResults = await newContext.CinemaProds.FirstOrDefaultAsync(_ => _.Id == prodId);
        var result = checkForResults?.AssignCinema == newCinemaProdCinema;
        
        Assert.True(result);
        response.EnsureSuccessStatusCode();
    }
}
