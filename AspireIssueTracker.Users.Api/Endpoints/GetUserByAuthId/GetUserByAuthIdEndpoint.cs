using AspireIssueTracker.Users.Data.Users;
using FastEndpoints;
using Functional;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspireIssueTracker.Users.Api.Endpoints.GetUserByAuthId
{
	public class GetUserByAuthIdEndpoint(IUserRepository repository) : Endpoint<GetUserByAuthIdRequest, Results<Ok<UserResponse>, NotFound>>
	{
		public override void Configure()
		{
			Get("/api/users/{AuthId}");
			Permissions("read:users", "edit:users");
		}

		public override async Task<Results<Ok<UserResponse>, NotFound>> ExecuteAsync(GetUserByAuthIdRequest req, CancellationToken ct)
		{
			var user = await repository.GetByAuthIdAsync(req.AuthId, ct);
			if (user.HasValue())
			{
				var u = user.ValueOrDefault()!;
				return TypedResults.Ok(new UserResponse
				{
					Id = u.Id,
					Email = u.Email,
					Username = u.Username,
					FirstName = u.FirstName,
					LastName = u.LastName,
					AuthId = u.AuthId
				});
			}
			else
				return TypedResults.NotFound();
		}
	}
}