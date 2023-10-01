using GWIZD.Core.Data;
using WebApi.Model;

namespace GWIZD.Model;

public class Subscription : IEntity<Guid>
{
	public Guid Id { get; set; }
	public Guid Version { get; set; }

	public string Username { get; set; }
	public NotificationType NotificationType { get; set; }
	public string Destination { get; set; }
	public TootType TootType { get; set; }
	public AnimalType? AnimalType { get; set; }
	public Location Location { get; set; }
	public double Radius { get; set; }
}