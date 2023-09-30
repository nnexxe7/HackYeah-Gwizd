using GWIZD.Model;
using Service;

namespace WebApi;

public class CleanupBackgroundService : BackgroundService
{
	private readonly ITootsService _tootsService;


	public CleanupBackgroundService(ITootsService tootsService)
	{
		_tootsService = tootsService;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Run(() =>
			{
				List<Toot> expired = _tootsService.FindExpired();
				foreach (Toot toot in expired)
				{
					_tootsService.Delete(toot);
				}
			});

			await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
			if (stoppingToken.IsCancellationRequested) break;
		}
	}
}