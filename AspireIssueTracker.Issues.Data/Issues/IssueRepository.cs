using Functional;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspireIssueTracker.Issues.Data.Issues
{
	public class IssueRepository(IMongoClient client) : IIssueRepository
	{
		private readonly IMongoCollection<Issue> _issueCollection = client.GetDatabase("issue-tracker").GetCollection<Issue>("issues");

		public async Task<Option<Issue>> AddCommentToIssueAsync(IssueComment comment, string issueId, CancellationToken cancellationToken = default)
		{
			var issue = await GetIssueByIdAsync(issueId, cancellationToken);

			return await issue.MatchAsync(
				async i =>
				{
					i.Comments.Add(comment);
					await _issueCollection.ReplaceOneAsync(Builders<Issue>.Filter.Eq(i => i.Id, ObjectId.Parse(issueId)), i, cancellationToken: cancellationToken);
					return Option.Some(i);
				},
				() => Task.FromResult(Option.None<Issue>())
			);
		}

		public async Task<Issue> CreateIssueAsync(Issue issue, CancellationToken cancellationToken = default)
		{
			await _issueCollection.InsertOneAsync(issue, cancellationToken: cancellationToken);
			return issue;
		}

		public Task<List<Issue>> GetAllIssuesAsync(CancellationToken cancellationToken = default)
			=> _issueCollection.Find(Builders<Issue>.Filter.Empty).ToListAsync(cancellationToken);

		public async Task<Option<Issue>> GetIssueByIdAsync(string id, CancellationToken cancellationToken = default)
		{
			var result = await _issueCollection.FindAsync(Builders<Issue>.Filter.Eq(i => i.Id, ObjectId.Parse(id)), cancellationToken: cancellationToken);
			var issue = await result.FirstOrDefaultAsync(cancellationToken);
			return Option.FromNullable(issue);
		}

		public Task<List<Issue>> GetUnclaimedIssuesAsync(CancellationToken cancellationToken = default)
			=> _issueCollection.Find(Builders<Issue>.Filter.Eq(i => i.AssignedTo, null) & Builders<Issue>.Filter.Eq(i => i.Status, IssueStatus.Open)).ToListAsync(cancellationToken);

		public async Task<Option<Issue>> UpdateIssueAsync(Issue issue, CancellationToken cancellationToken = default)
		{
			var result = await _issueCollection.ReplaceOneAsync(Builders<Issue>.Filter.Eq(i => i.Id, issue.Id), issue, cancellationToken: cancellationToken);
			return result.IsAcknowledged ? Option.Some(issue) : Option.None<Issue>();
		}
	}
}