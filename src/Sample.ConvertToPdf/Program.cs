using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenTelemetry()
    .ConfigureResource(builder =>
    {
        builder.AddService("PDF Converter");
    })
    .WithTracing(builder =>
    {
        builder.AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(builder =>
    {
        builder.AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });

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

app.UseAuthorization();


app.MapControllers();

app.Run();