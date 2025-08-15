// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

//TESTE DOMAIN
// using Domain;

// class Program
// {
//     static void Main()
//     {
//         var livro = new Catalog
//         {
//             ID = Guid.NewGuid(),
//             Title = "Livro de Teste",
//             Author = "Letícia",
//             Number_pages = 150,
//             Year = 2025,
//             Description = "Violet"
//         };

//         Console.WriteLine($"{livro.ID} - {livro.Title} ({livro.Author}, {livro.Number_pages} {livro.Year} {livro.Description})");
//     }
// }

using Storage;
using Domain;

class Program
{
    static void Main()
    {
        var repo = new CatalogRepository();

        var novoLivro = new Catalog
        {
            ID = Guid.NewGuid(),
            Title = "Outro Teste",
            Author = "Letícia",
            Number_pages = 229293,
            Year = 2025,
            Description = "SAIU MDSSSS!!!!",
            Publisher_ID = 1,
            Language_ID = "Português"
            
        };

        repo.Add(novoLivro);
        Console.WriteLine("Livro salvo no banco!");

        var livros = repo.GetAll();
        foreach (var l in livros)
        {
            Console.WriteLine($"{l.ID} - {l.Title} - {l.Author} - {l.Number_pages} - {l.Year} - {l.Description} - {l.Publisher_ID} - {l.Language_ID}");
        }
    }
}

