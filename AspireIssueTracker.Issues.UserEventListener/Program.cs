using AspireIssueTracker.Issues.UserEventListener;
using AspireIssueTracker.Issues.Data;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(cfg =>
{
	cfg.SetKebabCaseEndpointNameFormatter();

	// Consumers
	cfg.AddConsumer<UserCreatedEventConsumer>();
	cfg.AddConsumer<UserUpdatedEventConsumer>();

	cfg.SetJobConsumerOptions();

	cfg.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(new Uri(builder.Configuration.GetConnectionString("messaging")!));
		cfg.ConfigureEndpoints(context);
	});
});

builder.AddMongoDBClient("issuedb");
builder.Services.AddIssuesData();

var host = builder.Build();
host.Run();