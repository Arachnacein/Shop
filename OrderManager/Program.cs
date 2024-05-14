using AutoMapper;
using OrderManager.Data;
using OrderManager.Mappers;
using OrderManager.Services;
using OrderManager.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderMapper, OrderMapper>();

//db

var db_host = Environment.GetEnvironmentVariable("db_host");
var db_name = Environment.GetEnvironmentVariable("db_name");
var db_password = Environment.GetEnvironmentVariable("db_password");
var connString = $"Data Source={db_host};Initial Catalog={db_name};Persist Security Info=True;User ID=sa;Password={db_password};TrustServerCertificate=True;";
//var connString = $"Data Source={db_host};Initial Catalog={db_name};User ID=sa;Password={db_password};TrustServerCertificate=True;";

builder.Services.AddDbContext<OrderDbContext>(options => 
{
    options.UseSqlServer(connString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
