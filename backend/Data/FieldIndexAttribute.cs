namespace GWIZD.Core.Data;

[AttributeUsage(AttributeTargets.Property)]
public class FieldIndexAttribute : Attribute
{
	public bool CaseInsensitive { get; set; }

	public bool Unique { get; set; }
}