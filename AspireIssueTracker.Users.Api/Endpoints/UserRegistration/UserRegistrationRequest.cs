namespace AspireIssueTracker.Users.Api.Endpoints.UserRegistration
{
	public record UserRegistrationRequest
	{
		public required string Email { get; init; }
		public required string Username { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		public required string AuthId { get; init; }
	}
}