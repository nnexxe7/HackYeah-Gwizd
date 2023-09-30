namespace GWIZD.Model;

public class Location
{
	//Szerokość
	public double Latitude { get; set; }

	/// <summary>
	/// Długość geograficzna
	/// </summary>
	public double Longtitude { get; set; }


	public double CalculateDistance(Location location)
	{ 
		return Math.Sqrt(Math.Pow(location.Latitude - Latitude, 2) + Math.Pow(location.Longtitude - Longtitude, 2));
	}
}