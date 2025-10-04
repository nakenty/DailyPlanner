using DailyPlanner.Application.Interfaces;
using DailyPlanner.Infrastructure.Services;

// Note: Additional services are registered below.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services.  Replace these in‑memory implementations with
// real ones (e.g. those that persist data to OneDrive or a database) when ready.
builder.Services.AddSingleton<ITaskService, InMemoryTaskService>();
builder.Services.AddSingleton<INoteService, InMemoryNoteService>();
builder.Services.AddSingleton<IFavoriteLinkService, InMemoryFavoriteLinkService>();
builder.Services.AddSingleton<IKpiService, InMemoryKpiService>();
builder.Services.AddSingleton<ISearchService, InMemorySearchService>();

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