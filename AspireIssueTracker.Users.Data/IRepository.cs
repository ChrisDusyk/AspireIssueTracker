using Functional;

namespace AspireIssueTracker.Users.Data
{
	public interface IRepository<T, TId> where T : class
	{
		public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

		public Task<Option<T>> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

		public Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

		public Task<Option<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default);
	}
}