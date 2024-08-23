using AspireIssueTracker.Issues.Data.Issues;
using AspireIssueTracker.Issues.Data.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AspireIssueTracker.Issues.Data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddIssuesData(this IServiceCollection services)
		{
			services
				.AddScoped<IIssueRepository, IssueRepository>()
				.AddScoped<IUserRepository, UserRepository>();
			return services;
		}
	}
}