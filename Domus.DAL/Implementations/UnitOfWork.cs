using Domus.DAL.Interfaces;

namespace Domus.DAL.Implementations;

public class UnitOfWork : IUnitOfWork, IDisposable
{
	private readonly IAppDbContext _dbContext;

	public UnitOfWork(IAppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

    public void Commit()
    {
		_dbContext.SaveChanges();
    }

    public async Task CommitAsync()
    {
		await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
		try
		{
			GC.SuppressFinalize(this);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
    }

    public void Rollback()
    {
		Console.WriteLine("Transaction rollback");
    }

    public async Task RollbackAsync()
    {
		Console.WriteLine("Transaction rollback");
		await Task.CompletedTask;	
    }
}
