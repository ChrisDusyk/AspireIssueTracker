namespace AspireIssueTracker.Users.Api.Endpoints
{
	public record UserResponse
	{
		public Guid Id { get; init; }
		public required string AuthId { get; init; }
		public required string Username { get; init; }
		public required string Email { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
	}
}