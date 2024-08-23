var builder = DistributedApplication.CreateBuilder(args);

var authAuthority = builder.AddParameter("Auth0Authority");
var authAudience = builder.AddParameter("Auth0Audience");
var authDomain = builder.AddParameter("Auth0Domain");
var authClientId = builder.AddParameter("Auth0ClientId");

//var dbUsername = builder.AddParameter("DbUsername", secret: true);
//var dbPassword = builder.AddParameter("DbPassword", secret: true);

var postgres = builder
	.AddPostgres("postgres")
	.WithDataVolume("issue-tracker-postgres-db")
	.WithPgAdmin();
var userDb = postgres.AddDatabase("userdb");

var issuesDb = builder
	.AddMongoDB("mongo")
	.WithDataVolume("issue-tracker-mongo-db")
	.WithMongoExpress()
	.AddDatabase("issuedb");

var rabbitmq = builder
	.AddRabbitMQ("messaging")
	.WithImage("masstransit/rabbitmq");

var issuesApi = builder.AddProject<Projects.AspireIssueTracker_Issues_Api>("issues-api")
	.WithReference(issuesDb)
	.WithEnvironment("Auth0:Authority", authAuthority)
	.WithEnvironment("Auth0:Audience", authAudience);

var usersApi = builder.AddProject<Projects.AspireIssueTracker_Users_Api>("users-api")
	.WithReference(userDb)
	.WithReference(rabbitmq)
	.WithEnvironment("Auth0:Authority", authAuthority)
	.WithEnvironment("Auth0:Audience", authAudience);

var ui = builder.AddNpmApp("ui", "../AspireIssueTracker.UI", scriptName: "dev")
	.WithReference(issuesApi)
	.WithReference(usersApi)
	.WithEnvironment("BROWSER", "none")
	.WithEnvironment("VITE_AUTH0_DOMAIN", authDomain)
	.WithEnvironment("VITE_AUTH0_CLIENTID", authClientId)
	.WithEnvironment("VITE_AUTH0_AUTHORITY", authAuthority)
	.WithEnvironment("VITE_AUTH0_AUDIENCE", authAudience)
	.WithEnvironment("VITE_USERS_API_URL", usersApi.GetEndpoint("https"))
	.WithEnvironment("VITE_ISSUES_API_URL", issuesApi.GetEndpoint("https"))
	.WithEnvironment("VITE_OTEL_EXPORTER_OTLP_ENDPOINT", builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"])
	.WithHttpsEndpoint(isProxied: false, port: 3000)
	.WithExternalHttpEndpoints()
	.PublishAsDockerFile();

builder.AddProject<Projects.AspireIssueTracker_Issues_UserEventListener>("issues-usereventlistener")
	.WithReference(issuesDb)
	.WithReference(rabbitmq);

builder.Build().Run();