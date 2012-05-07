namespace Coral.Engine.Logger
{
    public interface ILoggerFactory
    {
        ILogger Create(string category);
    }
}
