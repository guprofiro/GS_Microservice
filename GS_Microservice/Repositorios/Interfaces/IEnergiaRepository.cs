using Domain;

namespace Repositorios.Interfaces
{
	public interface IEnergiaRepository
	{
		Task<IEnumerable<Energia>> ObterTodosRegistros();
		Task InserirRegistro(Energia energia);
	}
}