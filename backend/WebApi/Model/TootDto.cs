using GWIZD.Model;

namespace WebApi.Model;

public class TootDto
{
	public TootType Type { get; set; }

	public string Species { get; set; }

	public Location Location { get; set; }
}