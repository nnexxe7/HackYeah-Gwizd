using GWIZD.Core.Data;

namespace GWIZD.Model
{
	public class Toot : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid Version { get; set; }

		public DateTime SubmittedAt { get; set; }

		public TootType Type { get; set; }

		public string Species { get; set; }

		public Location Location { get; set; } 

		public List<Attachment> Attachments { get; set; }


		public Toot()
		{
			Attachments = new List<Attachment>();
		}
	}
}