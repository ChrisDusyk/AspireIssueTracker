using AspireIssueTracker.Issues.Data.Issues;
using Functional;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Issues.Data.Users
{
	internal class UserRepository(IMongoClient client) : IUserRepository
	{
		private readonly IMongoCollection<User> _userCollection = client.GetDatabase("issue-tracker").GetCollection<User>("users");

		public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
		{
			await _userCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
			return user;
		}

		public async Task<Option<User>> GetUserById(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await _userCollection.FindAsync(Builders<User>.Filter.Eq(i => i.UserId, id.ToString()), cancellationToken: cancellationToken);
			var user = await result.FirstOrDefaultAsync(cancellationToken);
			return Option.FromNullable(user);
		}

		public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
		{
			var result = await _userCollection.ReplaceOneAsync(Builders<User>.Filter.Eq(i => i.Id, user.Id), user, cancellationToken: cancellationToken);
			if (result.IsAcknowledged && result.ModifiedCount == 1)
			{
				return user;
			}
			else
			{
				throw new Exception("Failed to update user");
			}
		}
	}
}