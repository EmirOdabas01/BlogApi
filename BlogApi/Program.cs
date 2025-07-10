using BlogApi.BLL.Interfaces;
using BlogApi.BLL.Services;
using BlogApi.DAL;
using BlogApi.DAL.Interfaces;
using BlogApi.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "blogwebsite API", Version = "v1" });
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogApiContext>(options =>
options.UseSqlServer(connectionString));


builder.Services.AddScoped(typeof(IPostRepository), typeof(PostRepository));
builder.Services.AddScoped<IPostService, PostService>();
var columnOptions = new ColumnOptions
{
    Store = new Collection<StandardColumn>
    {
        StandardColumn.TimeStamp,
        StandardColumn.Level,
        StandardColumn.Message,
        StandardColumn.Exception,
        StandardColumn.Properties
    }
};

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true // tablo otomatik oluþturulsun
        },
        columnOptions: columnOptions
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "blogwebsite API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
