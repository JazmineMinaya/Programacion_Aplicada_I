using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Data.Context;
using RegistroEstudiantes.Data.Services;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler =
        ReferenceHandler.IgnoreCycles);

builder.Services.AddOpenApi();

var ConStr = builder.Configuration.GetConnectionString("SqlConStr");

builder.Services.AddDbContextFactory<Contexto>(o => o.UseSqlServer(ConStr));

builder.Services.AddTransient<EstudiantesService>();
builder.Services.AddTransient<LibrosService>();
builder.Services.AddTransient<PrestamosService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Registro de Estudiantes API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
