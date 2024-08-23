using AspireIssueTracker.Issues.Data.Issues;
using FastEndpoints;
using Functional;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public static class GetIssueById
	{
		public record Request
		{
			public required string Id { get; init; }
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
			public List<IssueCommentsResponse> Comments { get; init; } = [];
		}

		public static Response MapToIssueResopnse(this Issue issue) =>
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
				Comments = issue.Comments?.Select(c => c.MapToIssueCommentsResponse()).ToList() ?? []
			};
	}

	public class GetIssueByIdEndpoint(IIssueRepository issueRepository) : Endpoint<GetIssueById.Request, GetIssueById.Response>
	{
		public override void Configure()
		{
			Get("/api/issues/{Id}");
			Permissions("read:issues");
		}

		public override async Task HandleAsync(GetIssueById.Request req, CancellationToken cancellationToken)
		{
			var issue = await issueRepository.GetIssueByIdAsync(req.Id, cancellationToken);

			await issue.DoAsync(
				i => SendOkAsync(i.MapToIssueResopnse(), cancellationToken),
				() => SendNotFoundAsync(cancellationToken));
		}
	}
}