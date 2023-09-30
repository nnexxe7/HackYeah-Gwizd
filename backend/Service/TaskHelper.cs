namespace Service;

public static class TaskHelper
{
	public static void Repeat(Action action, int times = 10)
	{
		for (int i = 0; i < times; i++)
		{
			try
			{
				action();
				break;
			}
			catch
			{
				Thread.Sleep(500);
			}
		}
	}
}