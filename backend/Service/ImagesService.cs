using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using Amazon.S3;
using GWIZD.Core;
using Amazon.S3.Model;
using GWIZD.Model;

namespace Service;

public class ImagesService : IImagesService
{
	private readonly ISettingsProvider _settingsProvider;


	public ImagesService(ISettingsProvider settingsProvider)
	{
		_settingsProvider = settingsProvider;
	}

	public void DeleteContent(Toot toot)
	{
		if (toot.Attachments == null || toot.Attachments.Count == 0) return;

		string accessKey = _settingsProvider.Get("s3_accesskey");
		string secretKey = _settingsProvider.Get("s3_secretkey");
		string bucketName = _settingsProvider.Get("bucket_name");
		var amazonS3Client = new AmazonS3Client(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.EUCentral1);

		foreach (Attachment attachment in toot.Attachments.Where(n => n.Type == AttachmentType.Photo))
		{
			if (!string.IsNullOrEmpty(attachment.Parameter1))
			{
				DeleteObjectResponse? result = amazonS3Client.DeleteObjectAsync(bucketName, Path.GetFileName(attachment.Parameter1)).Result;
			}

			if (!string.IsNullOrEmpty(attachment.Parameter2))
			{
				DeleteObjectResponse? result = amazonS3Client.DeleteObjectAsync(bucketName, Path.GetFileName(attachment.Parameter2)).Result;
			}
		}
	}

	public string UploadContent(byte[] fileContent, string fileName)
	{
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