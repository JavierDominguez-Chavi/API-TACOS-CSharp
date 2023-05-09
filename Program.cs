using Microsoft.EntityFrameworkCore;
using TACOS.Business;
using TACOS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<TacosdbContext>(options =>
                options.UseMySql(
                    "server=localhost;database=tacosdb;uid=tacosUser;pwd=T4C05",
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"
                )));
builder.Services.AddScoped<IMenuMgt, MenuMgr>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
