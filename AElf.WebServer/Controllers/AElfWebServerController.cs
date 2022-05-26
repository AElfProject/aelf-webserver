using System.Collections.Generic;
using System.Threading.Tasks;
using AElf.Contracts.MultiToken;
using AElf.Types;
using AElf.WebServer.Dtos;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AElf.WebServer.Controllers;

[ApiController]
[Route("api")]
public class AElfWebServerController : ControllerBase
{
    private readonly IAElfClientService _clientService;
    private readonly ILogger<AElfWebServerController> _logger;
    private readonly ConfigOptions _configOptions;

    public AElfWebServerController(IAElfClientService clientService,
        IOptionsSnapshot<ConfigOptions> configOptions,
        ILogger<AElfWebServerController> logger)
    {
        _clientService = clientService;
        _logger = logger;
        _configOptions = configOptions.Value;
    }

    [HttpGet("getBalance")]
    public async Task<Dictionary<string, long>> GetBalance(string address)
    {
        var dict = new Dictionary<string, long>();

        foreach (var symbol in _configOptions.TokenList)
        {
            _logger.LogInformation($"Trying to get {symbol} balance of {address}");

            var result = await _clientService.ViewSystemAsync("AElf.ContractNames.Token", "GetBalance",
                new GetBalanceInput
                {
                    Owner = Address.FromBase58(address),
                    Symbol = symbol
                }, EndpointType.MainNetMainChain.ToString());
            var output = new GetBalanceOutput();
            output.MergeFrom(result);

            _logger.LogInformation($"Balance: {output.Balance}");

            dict.Add(symbol, output.Balance);
        }

        return dict;
    }
        
    [HttpPost("getBalanceBySymbol")]
    public async Task<long> GetBalance(GetBalanceInputDto input)
    {
        _logger.LogInformation($"Trying to get {input.Symbol} balance of {input.Address}");

        var result = await _clientService.ViewSystemAsync("AElf.ContractNames.Token", "GetBalance", new GetBalanceInput
        {
            Owner = Address.FromBase58(input.Address),
            Symbol = input.Symbol
        }, EndpointType.MainNetMainChain.ToString());
        var output = new GetBalanceOutput();
        output.MergeFrom(result);

        _logger.LogInformation($"Balance: {output.Balance}");

        return output.Balance;
    }
}