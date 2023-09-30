namespace GWIZD.Core.Data;

class FileBasedSettingsProvider : ISettingsProvider
{
	private readonly Lazy<Dictionary<string, string>> _connectionStrings;


	public FileBasedSettingsProvider()
	{
		_connectionStrings = new Lazy<Dictionary<string, string>>(ReadConfigFile);
	}

	private Dictionary<string, string> ReadConfigFile()
	{
		var settings  = new Dictionary<string, string>();

		string[] lines = File.ReadAllLines("C:\\GWIZD\\settings.conf");
		foreach (string line in lines)
		{
			string[] values = line.Split( "::");
			settings[values[0]] =  values[1];
		}

		return settings;
	}

	public string Get(string key)
	{
		if (_connectionStrings.Value.ContainsKey(key)) return _connectionStrings.Value[key];
		
		return null;
	}
}