using Monitoring.Moex.Infrastructure;
using Monitoring.Moex.WebApi.HostedServices;

var config = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json")
	.Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepos();
builder.Services.AddServices();
builder.Services.AddOther();
builder.Services.AddOptions(config);
builder.Services.AddDatabase(config);
builder.Services.AddRedisCache();

builder.Services.AddHostedService<LastTotalsMonitoringHostedService>();
builder.Services.AddHostedService<SecuritiesMonitoringHostedService>();
builder.Services.AddHostedService<EmailSenderHostedService>();

builder.Services.AddControllers();

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
