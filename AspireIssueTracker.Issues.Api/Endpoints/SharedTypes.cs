using AspireIssueTracker.Issues.Data.Issues;
using AspireIssueTracker.Issues.Data.Users;
using Functional;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public enum IssueStatus
	{
		Open,
		InProgress,
		Rejected,
		Resolved,
		Closed
	}

	public enum IssuePriority
	{
		Low,
		Medium,
		High,
		Critical
	}

	public record UserResponse
	{
		public Guid Id { get; init; }
		public string Name { get; init; } = "";
	}

	public record IssueCommentsResponse
	{
		public required Guid Id { get; init; }

		public int SortOrder { get; init; }
		public string Comment { get; init; } = "";
		public UserResponse? CommentedBy { get; init; }
		public DateTime CommentedAt { get; init; }
		public DateTime UpdatedAt { get; init; }
	}

	public static class Mappers
	{
		public static UserResponse MapToUserResponse(this User person) =>
			new()
			{
				Id = Guid.Parse(person.UserId),
				Name = person.Name,
			};

		public static T? ToNullable<T>(this Option<T> option)
			where T : class =>
			option.Match(
				some: value => value,
				none: () => null
			);

		public static IssueCommentsResponse MapToIssueCommentsResponse(this IssueComment comment)
			=> new()
			{
				Id = Guid.Parse(comment.Id),
				SortOrder = comment.SortOrder,
				Comment = comment.Comment,
				CommentedAt = comment.CommentedAt,
				CommentedBy = comment.CommentedBy?.MapToUserResponse(),
				UpdatedAt = comment.UpdatedAt
			};
	}
}