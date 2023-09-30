using GWIZD.Model;

namespace WebApi.Model;

public class FindByPointDto
{
	public Location Location { get; set; }
	public double Radius { get; set; }
}