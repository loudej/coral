using Coral.Engine.Logger;

namespace Coral.Engine.Tender
{
    public class ProcessTender : AbstractService, IProcessTender
    {
        public ProcessTender(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }

    public interface IProcessTender
    {

    }
}
