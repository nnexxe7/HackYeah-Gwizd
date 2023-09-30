namespace GWIZD.Core.Data;

public interface IEntity<T>
{
	T Id { get; set; }
	Guid Version { get; set; }
}