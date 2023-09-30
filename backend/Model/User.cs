using GWIZD.Core.Data;

namespace GWIZD.Model;

public class User : IEntity<string>
{
	public string Id { get; set; }
	public Guid Version { get; set; }
	[FieldIndex]
	public int Points { get; set; }
}