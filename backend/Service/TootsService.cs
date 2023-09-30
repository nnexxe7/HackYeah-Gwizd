using System.Drawing;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Amazon.S3;
using GWIZD.Core.Data;
using GWIZD.Model;
using MongoDB.Driver;
using Service.Repositories;

namespace Service;

public class TootsService : ITootsService
{
	private readonly ITootsRepository _repository;
	private readonly ISettingsProvider _settingsProvider;


	public TootsService(ITootsRepository repository, ISettingsProvider settingsProvider)
	{
		_repository = repository;
		_settingsProvider = settingsProvider;
	}

	public List<Toot> FindAll()
	{
		return _repository.Find(FilterDefinition<Toot>.Empty);
	}

	public List<Toot> FindByPoint(Location location, double radius)
	{
		if (location == null) throw new ArgumentNullException(nameof(location));
		List<Toot> toots = _repository.Find(FilterDefinition<Toot>.Empty); //this should be optimized

		return toots.Where(n => n.Location != null && n.Location.CalculateDistance(location) <= radius).ToList();
	}

	public Guid Submit(Toot toot)
	{
		return _repository.Save(toot);
	}

	public void AddPhotoAttachment(Guid tootId, byte[] fileContent, string extension)
	{
		if (fileContent == null) throw new ArgumentNullException(nameof(fileContent));

		Toot toot = _repository.Get(tootId);
		if (toot == null) throw new ArgumentException($"Toot with id:{tootId} does not exist");

		Task.Run(() =>
		{
			string miniaturePhotoUrl = string.Empty;
			//using (var stream = new MemoryStream(fileContent))
			//{
			//	Image img = new Bitmap(stream);
			//	Image miniature = img.GetThumbnailImage((int)img.Width / 5, (int)img.Width / 5, () => false, IntPtr.Zero);
			//	miniaturePhotoUrl = Upload(ImageToBytes(miniature), "_min" + extension);
			//}
			
			string originalPhotoUrl = Upload(fileContent, extension);

			TaskHelper.Repeat(() =>
			{
				Toot freshToot = _repository.Get(tootId);
				freshToot.Attachments.Add(new Attachment
				{
					AddedAt = DateTime.UtcNow,
					Type = AttachmentType.Photo,
					Parameter1 = originalPhotoUrl,
					Parameter2 = miniaturePhotoUrl
				});
				_repository.Save(freshToot);
			});
		});
	}

	private byte[] ImageToBytes(Image img)
	{
		using (var ms = new MemoryStream())
		{
			img.Save(ms, img.RawFormat);
			return ms.ToArray();
		}
	}

	private string Upload(byte[] fileContent, string extension)
	{
		const string bucketName = "gwizdphotos";
		Guid photoId = Guid.NewGuid();
		string fileName = photoId + extension;

		string accessKey = _settingsProvider.Get("s3_accesskey");
		string secretKey = _settingsProvider.Get("s3_secretkey");
		var amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.EUCentral1);

		var fileTransferUtility = new TransferUtility(amazonS3Client);
		using (var stream = new MemoryStream(fileContent))
		{
			fileTransferUtility.Upload(stream, bucketName, fileName);
		}

		return $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{fileName}";
	}
}