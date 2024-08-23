using AspireIssueTracker.Issues.Data.Issues;
using AspireIssueTracker.Issues.Data.Users;
using FastEndpoints;
using Functional;

namespace AspireIssueTracker.Issues.Api.Endpoints
{
	public static class AddComment
	{
		public record Request
		{
			[FromQueryParams()]
			public required string IssueId { get; init; }
			public string Comment { get; init; } = "";
			public Guid UserId { get; init; }
		}

		public static IssueCommentsResponse MapToCommentsResponse(this IssueComment comment)
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

	public class AddCommentEndpoint(IIssueRepository issueRepository, IUserRepository userRepository) : Endpoint<AddComment.Request, IssueCommentsResponse>
	{
		public override void Configure()
		{
			Post("/api/issues/{IssueId}/comments");
			Permissions("read:issues");
		}

		public override async Task HandleAsync(AddComment.Request request, CancellationToken cancellationToken)
		{
			var issue = await issueRepository.GetIssueByIdAsync(request.IssueId, cancellationToken);
			await issue.DoAsync(
				async i =>
				{
					var comment = await PersistCommentAsync(i, request, cancellationToken);
					await SendOkAsync(comment.MapToCommentsResponse(), cancellationToken);
				},
				() => SendNotFoundAsync(cancellationToken));
		}

		private async Task<IssueComment> PersistCommentAsync(Issue issue, AddComment.Request request, CancellationToken cancellationToken)
		{
			var user = await userRepository.GetUserById(request.UserId, cancellationToken);
			var comment = new IssueComment
			{
				Id = Guid.NewGuid().ToString(),
				SortOrder = issue.Comments.Count + 1,
				Comment = request.Comment,
				CommentedAt = DateTime.UtcNow,
				CommentedBy = user.ToNullable()
			};
			issue.Comments.Add(comment);
			await issueRepository.UpdateIssueAsync(issue, cancellationToken);
			return comment;
		}
	}
}