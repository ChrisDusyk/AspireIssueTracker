using AspireIssueTracker.Issues.Data.Issues;
using AspireIssueTracker.Issues.Data.Users;
using FastEndpoints;
using Functional;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public static class UpdateComment
	{
		public record Request
		{
			public required string Comment { get; init; }
			public required string UserId { get; init; }
		}
	}

	public class UpdateCommentEndpoint(IIssueRepository issueRepository, IUserRepository userRepository) : Endpoint<UpdateComment.Request, IssueCommentsResponse>
	{
		public override void Configure()
		{
			Post("/api/issues/{IssueId}/comments/{CommentId}");
			Permissions("read:issues");
		}

		public override async Task HandleAsync(UpdateComment.Request req, CancellationToken ct)
		{
			var issueId = Route<string>("IssueId");
			if (issueId is null)
				await SendErrorsAsync(cancellation: ct);

			var issue = await issueRepository.GetIssueByIdAsync(issueId!, ct);
			await issue.DoAsync(
				async i =>
				{
					var comment = await PersistCommentAsync(i, req, ct);
					await SendOkAsync(comment.MapToIssueCommentsResponse(), ct);
				},
				() => SendNotFoundAsync(ct));
		}

		private async Task<IssueComment> PersistCommentAsync(Issue issue, UpdateComment.Request request, CancellationToken cancellationToken)
		{
			var userId = Guid.Parse(request.UserId);
			var user = await userRepository.GetUserById(userId, cancellationToken);

			var commentId = Route<string>("CommentId");

			var comment = issue.Comments.FirstOrDefault(c => c.Id == commentId);
			var updatedComment = comment! with
			{
				Comment = request.Comment,
				UpdatedAt = DateTime.UtcNow
			};

			var index = issue.Comments.IndexOf(comment);
			issue.Comments[index] = updatedComment;

			await issueRepository.UpdateIssueAsync(issue, cancellationToken);
			return updatedComment;
		}
	}
}