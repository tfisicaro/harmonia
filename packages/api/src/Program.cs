using Harmonia.Api.Repositories;
using Harmonia.Api.Services;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", builder.Configuration.GetSection("OpenApiSettings").Get<OpenApiInfo>());
});
builder.Services.AddScoped<ShortUrlRepository>();
builder.Services.AddScoped<CosmosShortUrlService>();
builder.Services.AddSingleton(s => {
    var connectionString = builder.Configuration.GetConnectionString("Default");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException(
            "Please specify a valid Default connection string in the appsettings.json file or your Azure Configuration.");
    }
 
    return new CosmosClientBuilder(connectionString).Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
