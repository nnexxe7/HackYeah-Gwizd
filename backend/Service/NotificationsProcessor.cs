using GWIZD.Model;
using Service.Repositories;

namespace Service;

public class NotificationsProcessor : INotificationsProcessor
{
	private readonly ISubscriptionsRepository _subscriptionsRepository;
	private readonly INotificationsSender _notificationsService;


	public NotificationsProcessor(ISubscriptionsRepository subscriptionsRepository, INotificationsSender notificationsService)
	{
		_subscriptionsRepository = subscriptionsRepository;
		_notificationsService = notificationsService;
	}

	public void Process(Toot toot)
	{
		if (toot == null) throw new ArgumentNullException(nameof(toot));

		Task.Run(() =>
		{
			List<Subscription> subscriptions = _subscriptionsRepository.GetMatching(toot.Type, toot.RelatedAnimal, toot.Location);
			foreach (Subscription subscription in subscriptions)
			{
				Process(subscription);
			}
		});
	}

	private void Process(Subscription subscription)
	{
		switch (subscription.NotificationType)
		{
			case NotificationType.Sms:
				_notificationsService.SendSms(subscription.Destination, $"Hello {subscription.Username}!{Environment.NewLine}{subscription.TootType}/{subscription.AnimalType} in subscribed area!");
				break;
			default: throw new ArgumentOutOfRangeException();
		}
		
	}
}