using AspireIssueTracker.Issues.Data.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.Issues.Data.Issues
{
	public record Issue
	{
		public ObjectId Id { get; init; }

		public required string Title { get; init; }
		public string Description { get; init; } = "";
		public IssueStatus Status { get; init; }
		public IssuePriority Priority { get; init; }
		public User? AssignedTo { get; init; }
		public User? ReportedBy { get; init; }
		public DateTime CreatedAt { get; init; }
		public DateTime UpdatedAt { get; init; }
		public List<IssueComment> Comments { get; init; } = [];
	}

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

	// Potential future enhancement
	public enum IssueType
	{
		Bug,
		Feature,
		Task,
		Improvement
	}

	public record IssueComment
	{
		public required string Id { get; init; }

		public int SortOrder { get; init; }
		public string Comment { get; init; } = "";
		public User? CommentedBy { get; init; }
		public DateTime CommentedAt { get; init; }
		public DateTime UpdatedAt { get; init; }
	}
}