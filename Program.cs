using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using JWTTokens;
using System.Reflection;
using Microsoft.AspNetCore.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication();
builder.Services.AddControllers()
                .AddJsonOptions(x =>
                     x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
});


builder.Services.AddDbContext<TacosdbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("tacosdb"),
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.25-mysql"
                )));
builder.Services.AddScoped<IConsultanteMgt, ConsultanteMgr>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
