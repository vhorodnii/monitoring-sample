using MassTransit;
using Sample.Shared.FileStorage;
using Sample.Shared.Telemetry;
using Sample.Web.Consumers;
using Sample.Web.ProcessingTask;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true);

builder.Logging.AddApplicationLogging("App Gateway");
builder.Services.AddApplicationTelemetry("App Gateway");
builder.Services.AddLocalFileStorage(builder.Configuration);

builder.Services.AddSingleton<ITasksService, TasksService>();

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

    c.AddPublishObserver<TelemetryPropagationPublishObserver>();
    c.AddConsumer<NewTaskReceivedConsumer>();
    c.AddConsumer<DocumentCleanedConsumer>();
    c.AddConsumer<DocumentConvertedToPdfConsumer>();
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();


app.MapControllers();

app.Run();