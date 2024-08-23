using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireIssueTracker.MessageContracts
{
	public record UserUpdatedEvent
	{
		public Guid Id { get; init; }
		public string? FirstName { get; init; }
		public string? LastName { get; init; }
	}
}