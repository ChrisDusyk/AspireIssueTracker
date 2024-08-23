using Functional;
using Microsoft.EntityFrameworkCore;

namespace AspireIssueTracker.Users.Data.Users
{
	internal class UserRepository(UsersDbContext context) : IUserRepository
	{
		public async Task<User> CreateAsync(User entity, CancellationToken cancellationToken = default)
		{
			context.Users.Add(entity);
			await context.SaveChangesAsync(cancellationToken);
			return entity;
		}

		public Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
			context.Users.ToListAsync(cancellationToken);

		public async Task<Option<User>> GetByAuthIdAsync(string authId, CancellationToken cancellationToken = default)
		{
			var user = await context.Users.SingleAsync(u => u.AuthId == authId, cancellationToken: cancellationToken);
			return Option.FromNullable(user);
		}

		public async Task<Option<User>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		{
			var result = await context.Users.FindAsync([id], cancellationToken: cancellationToken);
			return Option.FromNullable(result);
		}

		public async Task<Option<User>> UpdateAsync(User entity, CancellationToken cancellationToken = default)
		{
			var user = await context.Users.FindAsync([entity.Id], cancellationToken: cancellationToken);
			if (user is null)
			{
				return Option.None<User>();
			}

			user.AuthId = entity.AuthId;
			user.FirstName = entity.FirstName;
			user.LastName = entity.LastName;
			user.UpdatedAt = DateTime.UtcNow;

			await context.SaveChangesAsync(cancellationToken);
			return Option.Some(user);
		}
	}
}