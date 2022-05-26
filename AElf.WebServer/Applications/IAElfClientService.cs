using System.Threading.Tasks;
using Google.Protobuf;

namespace AElf.WebServer;

public interface IAElfClientService
{
    Task<byte[]> ViewAsync(string contractAddress, string methodName, IMessage parameter, string clientAlias,
        string accountAlias = "Default");

    Task<byte[]> ViewSystemAsync(string systemContractName, string methodName, IMessage parameter,
        string clientAlias, string accountAlias = "Default");
}