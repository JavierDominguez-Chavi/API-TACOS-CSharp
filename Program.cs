using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using JWTTokens;
using System.Reflection;
using Microsoft.AspNetCore.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddJwtAuthentication();
builder.Services.AddControllers()
                .AddJsonOptions(x =>
                     x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
});

builder.Services.AddDbContext<TacosdbContext>(options =>
                options.UseMySql(
                    "server=localhost;database=tacosdb;uid=tacosUser;pwd=T4C05",
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"
                )));
builder.Services.AddScoped<IMenuMgt, MenuMgr>();
builder.Services.AddScoped<IConsultanteMgt, ConsultanteMgr>();
builder.Services.AddScoped<JwtTokenHandler>();

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
