using GWIZD.Core.Data;
using WebApi.Model;

namespace GWIZD.Model
{
	public class Toot : IEntity<Guid>
	{
		public Guid Id { get; set; }
		public Guid Version { get; set; }

		public DateTime SubmittedAt { get; set; }
		public DateTime ExpiresAt { get; set; }
		public string SubmittedBy { get; set; } //To refactor and use auth
		public TootType Type { get; set; }
		public AnimalType? RelatedAnimal { get; set; }
		public ActivityType? Activity { get; set; }
		public Location Location { get; set; }
		public string Description { get; set; }
		public bool IsDangerous { get; set; }
		public List<Attachment> Attachments { get; set; }


		public Toot()
		{
			Attachments = new List<Attachment>();
		}
	}
}