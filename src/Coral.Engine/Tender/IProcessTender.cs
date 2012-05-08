using System.Threading.Tasks;

namespace Coral.Engine.Tender
{
    public interface IProcessTender
    {
        Task AddProcess(ProcessDefinition definition);
        Task RemoveProcess(string processName);
    }
}
