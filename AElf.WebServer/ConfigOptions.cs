using System.Collections.Generic;

namespace AElf.WebServer
{
    public class ConfigOptions
    {
        public string AccountAddress { get; set; }
        public string BlockChainEndpoint { get; set; }
        public string TokenContractAddress { get; set; }
        public List<string> TokenList { get; set; }
    }
}