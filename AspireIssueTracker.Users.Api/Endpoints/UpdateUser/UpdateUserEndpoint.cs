using AspireIssueTracker.Users.Data.Users;
using FastEndpoints;
using Functional;

namespace AspireIssueTracker.Users.Api.Endpoints.UpdateUser
{
	public class UpdateUserEndpoint(IUserRepository userRepository) : Endpoint<UpdateUserRequest, UserResponse>
	{
		public override void Configure()
		{
			Post("/api/users/{UserId}/update");
			Permissions("edit:users");
		}

		public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
		{
			var userToUpdate = new User
			{
				Id = req.UserId,
				FirstName = req.FirstName,
				LastName = req.LastName,
				AuthId = req.AuthId
			};

			var userResult = await userRepository.UpdateAsync(userToUpdate, ct);

			await userResult.DoAsync(
				u => SendOkAsync(MapUserToResponse(u), ct),
				() => SendNotFoundAsync(ct));
		}

		private static UserResponse MapUserToResponse(User user) =>
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