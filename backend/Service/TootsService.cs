using System.Drawing;
using System.Drawing.Imaging;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Amazon.S3;
using GWIZD.Core.Data;
using GWIZD.Model;
using GWIZD.Service;
using MongoDB.Driver;
using Service.Repositories;

namespace Service;

public class TootsService : ITootsService
{
	private readonly ITootsRepository _repository;
	private readonly ISettingsProvider _settingsProvider;
	private readonly ITootsDuplicateDetector _duplicateDetector;


	public TootsService(ITootsRepository repository, ISettingsProvider settingsProvider, ITootsDuplicateDetector duplicateDetector)
	{
		_repository = repository;
		_settingsProvider = settingsProvider;
		_duplicateDetector = duplicateDetector;
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

	public Guid? Submit(Toot toot)
	{
		if (toot == null) throw new ArgumentNullException(nameof(toot));

		bool canSubmit = _duplicateDetector.CanSubmit(toot, out Toot? duplicate);
		if (canSubmit)
		{
			toot.ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromHours(Consts.ExpiryTime));
			return _repository.Save(toot);
		}

		if (duplicate != null)
		{
			duplicate.ExpiresAt = duplicate.ExpiresAt.Add(TimeSpan.FromHours(Consts.ExpiryBump));
			//TODO: async, it should be done via message bus
			_repository.Save(duplicate);
		}

		return null;
	}

	public void AddPhotoAttachment(Guid tootId, byte[] fileContent, string extension)
	{
		if (fileContent == null) throw new ArgumentNullException(nameof(fileContent));

		Toot toot = _repository.Get(tootId);
		if (toot == null) throw new ArgumentException($"Toot with id:{tootId} does not exist");

		Task.Run(() =>
		{
			string miniaturePhotoUrl = CreateAndUploadMiniature(fileContent, extension);
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

	private string CreateAndUploadMiniature(byte[] fileContent, string extension)
	{
		try
		{
			using (var stream = new MemoryStream(fileContent))
			{
				Image original = new Bitmap(stream);
				Image img = new Bitmap(original, new Size((int)original.Width / 5, (int)original.Height / 5));

				using (var reizedStream = new MemoryStream())
				{
					img.Save(reizedStream, ImageFormat.Jpeg);
					return Upload(reizedStream.ToArray(), "_min" + extension);
				}
			}
		}
		catch
		{
			return string.Empty;
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