using ClinicaEstetica.Data;
using ClinicaEstetica.Repositories;
using ClinicaEstetica.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=clinicaestetica.db"));

// Repositories
builder.Services.AddScoped<BiomedicoRepository>();
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<ProcedimentoRepository>();
builder.Services.AddScoped<AgendamentoRepository>();

// Services
builder.Services.AddScoped<BiomedicoService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProcedimentoService>();
builder.Services.AddScoped<AgendamentoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();