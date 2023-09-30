using System.Net.Mime;
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

	public void AddPhotoAttachment(Guid tootId, byte[] fileContent, string extension)
	{
		if (fileContent == null) throw new ArgumentNullException(nameof(fileContent));

		Toot toot = _repository.Get(tootId);
		if (toot == null) throw new ArgumentException($"Toot with id:{tootId} does not exist");

		Task.Run(() =>
		{
			//Image
			string photoUrl = Upload(fileContent, extension);
			Toot freshToot = _repository.Get(tootId);
			freshToot.Attachments.Add(new Attachment
			{
				AddedAt = DateTime.UtcNow,
				Type = AttachmentType.Photo,
				Parameter1 = photoUrl
			});
			_repository.Save(freshToot);
		});
	}

	private string Upload(byte[] fileContent, string extension)
	{
		const string bucketName = "gwizdphotos";
		Guid photoId = Guid.NewGuid();
		string fileName = photoId + extension;

		string accessKey = _settingsProvider.Get("s3_accesskey");
		string secretKey = _settingsProvider.Get("s3_secretkey");
		var amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey));

		var fileTransferUtility = new TransferUtility(amazonS3Client);
		using (var stream = new MemoryStream(fileContent))
		{
			fileTransferUtility.Upload(stream, bucketName, fileName);
		}

		return $"https://{bucketName}.s3.eu-central-1.amazonaws.com/{fileName}";
	}
}