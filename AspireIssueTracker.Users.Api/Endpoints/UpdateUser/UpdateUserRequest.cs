using FastEndpoints;

namespace AspireIssueTracker.Users.Api.Endpoints.UpdateUser
{
	public record UpdateUserRequest
	{
		[FromQueryParams]
		public Guid UserId { get; init; }

		public string FirstName { get; init; } = "";
		public string LastName { get; init; } = "";
		public string AuthId { get; init; } = "";
	}
}