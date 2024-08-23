using AspireIssueTracker.Users.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace AspireIssueTracker.Users.Data
{
	public class UsersDbContext(DbContextOptions options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
	}
}