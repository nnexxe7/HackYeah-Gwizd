using NLog;

namespace GWIZD.Core;

public class NLogBasedLogger<T> : ILog<T>
{
	private readonly Logger _logger;


	public NLogBasedLogger()
	{
		_logger = LogManager.GetLogger(typeof(T).Name);
	}

	/// <inheritdoc/>
	public void Trace(string message)
	{
		_logger.Trace(message);
	}

	/// <inheritdoc/>
	public void Info(string message)
	{
		_logger.Info(message);
	}

	/// <inheritdoc/>
	public void Warn(string message)
	{
		_logger.Warn(message);
	}

	/// <inheritdoc/>
	public void Error(string message, Exception e = null)
	{
		_logger.Error(e, message);
	}
}