using System.Drawing;
using System.Drawing.Imaging;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Amazon.S3;
using GWIZD.Core;
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
	private readonly IUsersService _usersService;


	public TootsService(ITootsRepository repository, ISettingsProvider settingsProvider, ITootsDuplicateDetector duplicateDetector, IUsersService usersService)
	{
		_repository = repository;
		_settingsProvider = settingsProvider;
		_duplicateDetector = duplicateDetector;
		_usersService = usersService;
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
			_usersService.IncreasePoints(toot.SubmittedBy, Consts.PointsForToot);

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

	public void AddPhotoAttachment(Guid tootId, string submittedBy, byte[] fileContent, string extension)
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
					Parameter2 = miniaturePhotoUrl,
					SubmittedBy = submittedBy
				});
				_repository.Save(freshToot);
				_usersService.IncreasePoints(submittedBy, Consts.PointsForPhoto);
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
		Guid photoId = Guid.NewGuid();
		string fileName = photoId + extension;

		string accessKey = _settingsProvider.Get("s3_accesskey");
		string secretKey = _settingsProvider.Get("s3_secretkey");
		var amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.EUCentral1);

		var fileTransferUtility = new TransferUtility(amazonS3Client);
		using (var stream = new MemoryStream(fileContent))
		{
			fileTransferUtility.Upload(stream, _settingsProvider.Get("bucket_name"), fileName);
		}

		return $"https://{_settingsProvider.Get("cloud_front")}/{fileName}";
	}
}