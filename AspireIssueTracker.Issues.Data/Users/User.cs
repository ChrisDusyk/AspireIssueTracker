using MongoDB.Bson;

namespace AspireIssueTracker.Issues.Data.Users
{
	public record User
	{
		public ObjectId Id { get; init; }
		public required string UserId { get; init; }
		public string Name { get; init; } = "";
	}
}