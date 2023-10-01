using GWIZD.Core;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Service;

public class NotificationsSender : INotificationsSender
{
	private readonly string _twillioNumber;


	public NotificationsSender(ISettingsProvider settingsProvider)
	{
		TwilioClient.Init(settingsProvider.Get("twillio_sid"), settingsProvider.Get("twillio_authtoken"));
		_twillioNumber = settingsProvider.Get("twillio_number");
	}

	public void SendSms(string to, string message)
	{
		if (to == null) throw new ArgumentNullException(nameof(to));
		if (message == null) throw new ArgumentNullException(nameof(message));

		MessageResource.Create(body: message, to: new PhoneNumber(to), from: _twillioNumber);
	}
}