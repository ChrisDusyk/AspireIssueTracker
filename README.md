# AspireIssueTracker

This is a sample of doing microservices in a monorepo architecture using [.NET Aspire](https://learn.microsoft.com/en-ca/dotnet/aspire/)

## Setup Instructions

### Set up `dotnet dev-certs`

This is required to load the ASP.NET dev certificates for SSL 

1. Navigate to `ApiService` project folder in command line
2. Run `dotnet dev-certs https -ep "{path to root folder}\ApsireIssueTracker.UI\certificates\aspnet_https.pem" --format pem -v -np`
