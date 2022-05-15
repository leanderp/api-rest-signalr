using api.Context;
using api.Hubs;
using api.MiddlewareExtensions;
using api.Repository;
using api.SubscribeTableDependencies;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// DT
builder.Services.AddSingleton<DashboardHub>();
builder.Services.AddSingleton<SubscribePostTableDependency>();

// DB and Repository
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Entities and DTOss
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
var connectionString = app.Configuration.GetConnectionString("DefaultConnection");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<DashboardHub>("/dashboard");

app.MapControllers();

// Migration
var scope = app.Services.CreateScope();
await AppDBContext.Migrations(scope.ServiceProvider);

// Middleware
app.UseSqlTableDependency<SubscribePostTableDependency>(connectionString);

app.Run();


