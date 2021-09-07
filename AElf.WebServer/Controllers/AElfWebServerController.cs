using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Client.Dto;
using AElf.Contracts.MultiToken;
using AElf.Types;
using AElf.WebServer.Dtos;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AElf.WebServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class AElfWebServerController : ControllerBase
    {
        private readonly ILogger<AElfWebServerController> _logger;
        private readonly ConfigOptions _configOptions;

        public AElfWebServerController(IOptionsSnapshot<ConfigOptions> configOptions,
            ILogger<AElfWebServerController> logger)
        {
            _logger = logger;
            _configOptions = configOptions.Value;
        }

        [HttpGet("getBalance")]
        public async Task<Dictionary<string, long>> GetBalance(string address)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            var dict = new Dictionary<string, long>();

            foreach (var symbol in _configOptions.TokenList)
            {
                _logger.LogInformation($"Trying to get {symbol} balance of {address}");

                var tx = nodeManager.GenerateRawTransaction(_configOptions.AccountAddress,
                    _configOptions.TokenContractAddress,
                    "GetBalance", new GetBalanceInput
                    {
                        Owner = Address.FromBase58(address),
                        Symbol = symbol
                    });
                var resultBridge = await nodeManager.ApiClient.ExecuteTransactionAsync(new ExecuteTransactionDto
                {
                    RawTransaction = tx
                });
                var output = new GetBalanceOutput();
                output.MergeFrom(
                    ByteString.CopyFrom(ByteArrayHelper.HexStringToByteArray(resultBridge)));

                _logger.LogInformation($"Balance: {output.Balance}");

                dict.Add(symbol, output.Balance);
            }

            return dict;
        }
        
        [HttpPost("getBalanceBySymbol")]
        public async Task<long> GetBalance(GetBalanceInputDto input)
        {
            var nodeManager = new NodeManager(_configOptions.BlockChainEndpoint);
            _logger.LogInformation($"Trying to get {input.Symbol} balance of {input.Address}");

            var tx = nodeManager.GenerateRawTransaction(_configOptions.AccountAddress,
                _configOptions.TokenContractAddress,
                "GetBalance", new GetBalanceInput
                {
                    Owner = Address.FromBase58(input.Address),
                    Symbol = input.Symbol
                });
            var resultBridge = await nodeManager.ApiClient.ExecuteTransactionAsync(new ExecuteTransactionDto
            {
                RawTransaction = tx
            });
            var output = new GetBalanceOutput();
            output.MergeFrom(
                ByteString.CopyFrom(ByteArrayHelper.HexStringToByteArray(resultBridge)));

            _logger.LogInformation($"Balance: {output.Balance}");

            return output.Balance;
        }
    }
}