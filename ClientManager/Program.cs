using ClientManager.Data;
using ClientManager.Data.Repositories;
using ClientManager.Infrastructure.Repositories;
using ClientManager.Mappers;
using ClientManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDbContext<ClientDbContext>(options => 
    options.UseInMemoryDatabase("InMem"));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();


// my mappers
builder.Services.AddScoped<IClientMapper, ClientMapper>();
//############



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

InitDb.InitData(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
