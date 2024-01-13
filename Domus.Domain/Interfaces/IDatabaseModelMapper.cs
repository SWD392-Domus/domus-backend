using Microsoft.EntityFrameworkCore;

namespace Domus.Domain.Interfaces;

public interface IDatabaseModelMapper
{
	void Map(ModelBuilder modelBuilder);
}
