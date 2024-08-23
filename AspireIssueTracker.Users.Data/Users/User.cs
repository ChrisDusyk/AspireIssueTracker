using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspireIssueTracker.Users.Data.Users
{
	[Table("users")]
	public class User
	{
		[Key]
		[Column("internal_id")]
		public Guid Id { get; set; }

		[Column("auth_id")]
		public string AuthId { get; set; } = "";

		[Column("username")]
		public string Username { get; set; } = "";

		[Column("email")]
		public string Email { get; set; } = "";

		[Column("first_name")]
		public string? FirstName { get; set; }

		[Column("last_name")]
		public string? LastName { get; set; }

		[Column("created_at")]
		public DateTime CreatedAt { get; set; }

		[Column("updated_at")]
		public DateTime UpdatedAt { get; set; }
	}
}