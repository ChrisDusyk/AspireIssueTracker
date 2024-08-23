using AspireIssueTracker.MessageContracts;
using AspireIssueTracker.Users.Data.Users;
using FastEndpoints;
using MassTransit;

namespace AspireIssueTracker.Users.Api.Endpoints.UserRegistration
{
	public class UserRegistrationEndpoint(IUserRepository repository, IPublishEndpoint publishEndpoint) : Endpoint<UserRegistrationRequest, UserRegistrationResponse>
	{
		public override void Configure()
		{
			Post("/api/users");
			Permissions("edit:users");
		}

		public override async Task HandleAsync(UserRegistrationRequest req, CancellationToken ct)
		{
			var now = DateTime.UtcNow;
			var user = new User
			{
				Email = req.Email,
				Username = req.Username,
				FirstName = req.FirstName,
				LastName = req.LastName,
				AuthId = req.AuthId,
				Id = Guid.NewGuid(),
				CreatedAt = now,
				UpdatedAt = now
			};

			var createdUser = await repository.CreateAsync(user, ct);

			// Broadcast the user created event for other services to consume
			var userCreatedEvent = new UserCreatedEvent
			{
				Id = createdUser.Id,
				Email = createdUser.Email,
				FirstName = createdUser.FirstName,
				LastName = createdUser.LastName,
				AuthId = createdUser.AuthId
			};
			await publishEndpoint.Publish(userCreatedEvent, ct);

			await SendOkAsync(new UserRegistrationResponse { Id = createdUser.Id }, ct);
		}
	}
}