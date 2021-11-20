using Monitoring.Moex.Infrastructure;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRepos();
builder.Services.AddServices();
builder.Services.AddHelpers();
builder.Services.AddHostedServices();
builder.Services.AddOptions(config);
builder.Services.AddDatabase(config);
builder.Services.AddQueryHandlers();
builder.Services.AddRedisCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
