using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;

namespace V9AgentInfo.Hubs
{
    public interface ISignalR
    {
        Task ListInfo(Info info);
    }
}