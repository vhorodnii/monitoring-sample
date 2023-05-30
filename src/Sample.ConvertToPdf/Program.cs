using MassTransit;
using Sample.ConvertToPdf.Converter;
using Sample.Infrastucture;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddApplicationLogging("PDF Converter");
builder.Services.AddApplicationTelemetry("PDF Converter");
builder.Services.AddLocalFileStorage(builder.Configuration);
builder.Services.AddMassTransit(c =>
{
    c.SetKebabCaseEndpointNameFormatter();
    c.UsingRabbitMq((bus, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", x =>
        {
            x.Username("myuser");
            x.Password("mypassword");
        });
        cfg.Durable = true;
        cfg.ConfigureEndpoints(bus);
    });
    c.AddConsumer<ConvertToPdfConsumer>();
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