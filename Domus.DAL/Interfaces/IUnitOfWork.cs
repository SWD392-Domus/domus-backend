using Domus.Common.Interfaces;

namespace Domus.DAL.Interfaces;

public interface IUnitOfWork : IAutoRegisterable
{
	void Commit();
	Task CommitAsync();
	void Rollback();
	Task RollbackAsync();
}
