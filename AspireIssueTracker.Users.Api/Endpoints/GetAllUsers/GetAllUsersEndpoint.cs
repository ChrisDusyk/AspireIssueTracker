using AspireIssueTracker.Users.Data.Users;
using FastEndpoints;

namespace AspireIssueTracker.Users.Api.Endpoints.GetAllUsers
{
	public class GetAllUsersEndpoint(IUserRepository userRepository) : EndpointWithoutRequest<IEnumerable<UserResponse>>
	{
		public override void Configure()
		{
			Get("/api/users");
			Permissions("read:users", "edit:users");
		}

		public override async Task HandleAsync(CancellationToken cancellationToken)
		{
			var users = await userRepository.GetAllAsync(cancellationToken);
			await SendOkAsync(users.Select(MapUserToResponse), cancellationToken);
		}

		private UserResponse MapUserToResponse(User user) =>
			new()
			{
				Id = user.Id,
				AuthId = user.AuthId,
				Email = user.Email,
				Username = user.Username,
				FirstName = user.FirstName,
				LastName = user.LastName,
			};
	}
}