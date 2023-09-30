using System.Diagnostics;
using GWIZD.Model;

namespace WebApi.Model;

public class TootDto
{
	public TootType Type { get; set; }
	public AnimalType? RelatedAnimal { get; set; }
	public ActivityType? Activity { get; set; }
	public Location Location { get; set; }
	public string Description { get; set; }
	public bool IsDangerous { get; set; }
	public string SubmittedBy { get; set; } //To refactor and use auth
}