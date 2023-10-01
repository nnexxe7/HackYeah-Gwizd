namespace Service;

public interface INotificationsSender
{
	void SendSms(string to, string message);
}