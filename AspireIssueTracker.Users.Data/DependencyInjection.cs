using AspireIssueTracker.Users.Data.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Users.Data
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddUsersData(this IServiceCollection services)
		{
			services.AddScoped<IUserRepository, UserRepository>();
			return services;
		}
	}
}