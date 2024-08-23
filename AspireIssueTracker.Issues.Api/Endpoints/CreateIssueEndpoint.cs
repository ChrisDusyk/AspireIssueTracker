using AspireIssueTracker.Issues.Api.Endpoints;
using AspireIssueTracker.Issues.Data.Issues;
using AspireIssueTracker.Issues.Data.Users;
using FastEndpoints;
using Functional;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public static class CreateIssue
	{
		public record Request
		{
			public required string Title { get; init; }
			public string Description { get; init; } = "";
			public IssueStatus Status { get; init; } = IssueStatus.Open;
			public IssuePriority Priority { get; init; } = IssuePriority.Low;
			public Guid? AssignedTo { get; init; }
			public Guid? ReportedBy { get; init; }
		}

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
			public List<CommentResponse> Comments { get; init; } = [];
		}

		public record CommentResponse
		{
			public required string Id { get; init; }

			public int SortOrder { get; init; }
			public string Comment { get; init; } = "";
			public UserResponse? CommentedBy { get; init; }
			public DateTime CommentedAt { get; init; }
			public DateTime UpdatedAt { get; init; }
		}

		public static Issue MapToIssue(this Request request, Option<User> assignedTo, Option<User> reportedBy) =>
			new()
			{
				Title = request.Title,
				Description = request.Description,
				Status = (Data.Issues.IssueStatus)request.Status,
				Priority = (Data.Issues.IssuePriority)request.Priority,
				AssignedTo = assignedTo.ToNullable(),
				ReportedBy = reportedBy.ToNullable(),
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Comments = []
			};

		public static CommentResponse MapToCreatedIssueCommentResponse(this IssueComment comment) =>
			new()
			{
				Id = comment.Id.ToString(),
				SortOrder = comment.SortOrder,
				Comment = comment.Comment,
				CommentedAt = comment.CommentedAt,
				CommentedBy = comment.CommentedBy?.MapToUserResponse(),
				UpdatedAt = comment.UpdatedAt
			};

		public static Response MapToCreatedIssueResponse(this Issue issue) =>
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
				UpdatedAt = issue.UpdatedAt
			};
	}

	public class CreateIssueEndpoint(IIssueRepository issueRepository, IUserRepository userRepository) : Endpoint<CreateIssue.Request, CreateIssue.Response>
	{
		public override void Configure()
		{
			Post("/api/issues");
			Permissions("read:issues");
		}

		public override async Task HandleAsync(CreateIssue.Request request, CancellationToken cancellationToken)
		{
			var assignedTo = await Option.FromNullable(request.AssignedTo).BindAsync(id => userRepository.GetUserById(id, cancellationToken));
			var reportedBy = await Option.FromNullable(request.ReportedBy).BindAsync(id => userRepository.GetUserById(id, cancellationToken));
			var issue = request.MapToIssue(assignedTo, reportedBy);

			var createdIssue = await issueRepository.CreateIssueAsync(issue, cancellationToken);

			var response = issue.MapToCreatedIssueResponse();
			//await SendCreatedAtAsync($"/api/issues/{response.Id}", response, cancellation: cancellationToken);
			await SendOkAsync(response, cancellationToken);
		}
	}
}