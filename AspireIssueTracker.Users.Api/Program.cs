using AspireIssueTracker.Users.Data;
using FastEndpoints;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddNpgsqlDbContext<UsersDbContext>("userdb");

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDocument(opts =>
{
	opts.Version = "v1";
});

builder.Services
	.AddAuthentication(opts =>
	{
		opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(opts =>
	{
		opts.Authority = builder.Configuration["Auth0:Authority"];
		opts.Audience = builder.Configuration["Auth0:Audience"];
	});
builder.Services.AddAuthorization();

builder.Services.AddCors(opts =>
{
	opts.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader();
		policy.AllowAnyMethod();
		policy.AllowAnyOrigin();
	});
});

builder.Services.AddUsersData();

builder.Services.AddMassTransit(cfg =>
{
	cfg.SetKebabCaseEndpointNameFormatter();

	// Consumers

	cfg.SetJobConsumerOptions();

	cfg.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(new Uri(builder.Configuration.GetConnectionString("messaging")!));
		cfg.ConfigureEndpoints(context);
	});
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(opts =>
	{
		opts.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
		opts.DefaultModelExpandDepth(0);
	});

	using var scope = app.Services.CreateScope();

	await ApplyMigrations<UsersDbContext>(scope);
}

app.UseFastEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.Run();

static async Task ApplyMigrations<TDbContext>(IServiceScope scope) where TDbContext : DbContext
{
	using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<TDbContext>>();

	logger.LogInformation("Applying migrations to Users database");
	var strategy = dbContext.Database.CreateExecutionStrategy();

	await strategy.ExecuteAsync(async () =>
	{
		await dbContext.Database.MigrateAsync();
		logger.LogInformation("Migrations successfully applied to Users database");
	});
}