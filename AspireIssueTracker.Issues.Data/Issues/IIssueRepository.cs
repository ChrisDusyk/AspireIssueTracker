using Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Issues.Data.Issues
{
	public interface IIssueRepository
	{
		public Task<List<Issue>> GetAllIssuesAsync(CancellationToken cancellationToken = default);

		public Task<List<Issue>> GetUnclaimedIssuesAsync(CancellationToken cancellationToken = default);

		public Task<Option<Issue>> GetIssueByIdAsync(string id, CancellationToken cancellationToken = default);

		public Task<Issue> CreateIssueAsync(Issue issue, CancellationToken cancellationToken = default);

		public Task<Option<Issue>> UpdateIssueAsync(Issue issue, CancellationToken cancellationToken = default);

		public Task<Option<Issue>> AddCommentToIssueAsync(IssueComment comment, string issueId, CancellationToken cancellationToken = default);
	}
}