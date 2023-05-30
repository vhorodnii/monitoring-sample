using MassTransit;
using Sample.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Logging.AddApplicationLogging("App Gateway");
builder.Services.AddApplicationTelemetry("App Gateway");
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

        cfg.ConfigureEndpoints(bus);
    });
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