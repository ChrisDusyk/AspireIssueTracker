using Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Issues.Data.Users
{
	public interface IUserRepository
	{
		public Task<Option<User>> GetUserById(Guid id, CancellationToken cancellationToken = default);

		public Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);

		public Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
	}
}