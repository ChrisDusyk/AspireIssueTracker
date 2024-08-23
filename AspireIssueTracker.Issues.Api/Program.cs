using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AspireIssueTracker.Issues.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddFastEndpoints();

builder.Services.AddCors(opts =>
{
	opts.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader();
		policy.AllowAnyMethod();
		policy.AllowAnyOrigin();
	});
});

builder.AddMongoDBClient("issuedb");

builder.Services.AddIssuesData();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.UseCors();

app.Run();