using AspireIssueTracker.Issues.Data.Users;
using AspireIssueTracker.MessageContracts;
using MassTransit;

namespace AspireIssueTracker.Issues.UserEventListener
{
	internal class UserCreatedEventConsumer(IUserRepository userRepository, ILogger<UserCreatedEventConsumer> logger) : IConsumer<UserCreatedEvent>
	{
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			var user = new User
			{
				UserId = context.Message.Id.ToString(),
				Name = string.IsNullOrWhiteSpace(context.Message.FirstName) ? context.Message.Email : $"{context.Message.FirstName} {context.Message.LastName}",
			};

			await userRepository.CreateUserAsync(user, cancellationToken: context.CancellationToken);

			logger.LogInformation("User created from {EventName}: {UserId}", nameof(UserCreatedEvent), user.UserId);
		}
	}
}