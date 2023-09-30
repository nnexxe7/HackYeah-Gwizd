namespace GWIZD.Core;

public interface ILog<T>
{
	void Trace(string message);

	void Info(string message);

	void Warn(string message);

	void Error(string message, Exception e = null);
}