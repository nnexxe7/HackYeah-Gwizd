namespace GWIZD.Core.Data;

public interface IGenericRepository<T, K> where T : IEntity<K>
{
	T Get(K id);

	K Save(T entity);
}