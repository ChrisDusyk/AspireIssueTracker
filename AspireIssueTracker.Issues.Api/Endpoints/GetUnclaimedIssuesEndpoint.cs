using AspireIssueTracker.Issues.Data.Issues;
using FastEndpoints;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public static class GetUnclaimedIssues
	{
		public record Response
		{
			public required string Id { get; init; }
			public required string Title { get; init; }
			public string Description { get; init; } = "";
			public IssueStatus Status { get; init; } = IssueStatus.Open;
			public IssuePriority Priority { get; init; } = IssuePriority.Low;
			public UserResponse? AssignedTo { get; init; }
			public UserResponse? ReportedBy { get; init; }
			public DateTime CreatedAt { get; init; }
			public DateTime UpdatedAt { get; init; }
			public List<CommentsResponse> Comments { get; init; } = [];
		}

		public record CommentsResponse
		{
			public required Guid Id { get; init; }

			public int SortOrder { get; init; }
			public string Comment { get; init; } = "";
			public UserResponse? CommentedBy { get; init; }
			public DateTime CommentedAt { get; init; }
			public DateTime UpdatedAt { get; init; }
		}

		public static Response MapToUnclaimedIssuesResponse(this Issue issue) =>
			new()
			{
				Id = issue.Id.ToString(),
				Title = issue.Title,
				Description = issue.Description,
				Status = (IssueStatus)issue.Status,
				Priority = (IssuePriority)issue.Priority,
				AssignedTo = issue.AssignedTo?.MapToUserResponse(),
				ReportedBy = issue.ReportedBy?.MapToUserResponse(),
				CreatedAt = issue.CreatedAt,
				UpdatedAt = issue.UpdatedAt,
				Comments = issue.Comments?.Select(c => c.MapToUnclaimedIssueCommentsResponse()).ToList() ?? []
			};

		public static CommentsResponse MapToUnclaimedIssueCommentsResponse(this IssueComment comment) =>
			new()
			{
				Id = Guid.Parse(comment.Id),
				SortOrder = comment.SortOrder,
				Comment = comment.Comment,
				CommentedAt = comment.CommentedAt,
				CommentedBy = comment.CommentedBy?.MapToUserResponse(),
				UpdatedAt = comment.UpdatedAt
			};
	}

	public class GetUnclaimedIssuesEndpoint(IIssueRepository issueRepository) : EndpointWithoutRequest<IEnumerable<GetUnclaimedIssues.Response>>
	{
		public override void Configure()
		{
			Get("/api/issues/unclaimed");
			Permissions("read:issues");
		}

		public override async Task HandleAsync(CancellationToken cancellationToken)
		{
			var issues = await issueRepository.GetUnclaimedIssuesAsync(cancellationToken);
			var response = issues.Select(i => i.MapToUnclaimedIssuesResponse());
			await SendOkAsync(response, cancellationToken);
		}
	}
}