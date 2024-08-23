using Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Users.Data.Users
{
	public interface IUserRepository : IRepository<User, Guid>
	{
		public Task<Option<User>> GetByAuthIdAsync(string authId, CancellationToken cancellationToken = default);
	}
}