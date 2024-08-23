namespace AspireIssueTracker.Users.Api.Endpoints.GetUserByAuthId
{
	public record GetUserByAuthIdRequest
	{
		public required string AuthId { get; init; }
	}
}