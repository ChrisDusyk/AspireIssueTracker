namespace AspireIssueTracker.MessageContracts
{
	public record UserCreatedEvent
	{
		public Guid Id { get; init; }
		public required string AuthId { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
		public string Email { get; init; } = "";
	}
}