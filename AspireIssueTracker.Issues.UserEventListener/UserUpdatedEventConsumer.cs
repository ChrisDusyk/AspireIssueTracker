using AspireIssueTracker.Issues.Data.Users;
using AspireIssueTracker.MessageContracts;
using Functional;
using MassTransit;

namespace AspireIssueTracker.Issues.UserEventListener
{
	internal class UserUpdatedEventConsumer(IUserRepository userRepository, ILogger<UserUpdatedEventConsumer> logger) : IConsumer<UserUpdatedEvent>
	{
		public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
		{
			var user = await userRepository.GetUserById(context.Message.Id, context.CancellationToken);
			await user.DoAsync(
				async u =>
				{
					var updatedUser = u with
					{
						Name = $"{context.Message.FirstName} {context.Message.LastName}"
					};
					await userRepository.UpdateUserAsync(u, context.CancellationToken);
					logger.LogInformation("User updated from {EventName}: {UserId}", nameof(UserUpdatedEvent), u.UserId);
				},
				() =>
				{
					logger.LogWarning("User not found: {UserId}", context.Message.Id);
					return Task.CompletedTask;
				});
		}
	}
}