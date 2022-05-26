using System;
using System.Collections.Generic;
using System.Linq;
using AElf.Client;
using AElf.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace AElf.WebServer;

public interface IAElfAccountProvider
{
    byte[] GetPrivateKey(string? alias = null, string? address = null);
    void SetPrivateKey(byte[] privateKey, string? alias = null, string? address = null);
}

public class AElfAccountProvider : Dictionary<AElfAccountInfo, byte[]>, IAElfAccountProvider, ISingletonDependency
{
    public ILogger<AElfAccountProvider> Logger { get; set; }

    public AElfAccountProvider()
    {
        Logger = NullLogger<AElfAccountProvider>.Instance;
        var defaultPrivateKey = ByteArrayHelper.HexStringToByteArray(AElfWebServerConstants.DefaultPrivateKey);
        SetPrivateKey(defaultPrivateKey, "Default", Address.FromPublicKey(defaultPrivateKey).ToBase58());
    }

    public byte[] GetPrivateKey(string? alias = null, string? address = null)
    {
        var keys = Keys
            .WhereIf(!alias.IsNullOrWhiteSpace(), a => a.Alias == alias)
            .WhereIf(!address.IsNullOrWhiteSpace(), a => a.Address == address)
            .ToList();
        if (keys.Count != 1)
        {
            throw new AElfClientException($"Failed to get private key of {alias} - {address}.");
        }

        return this[keys.Single()];
    }

    public void SetPrivateKey(byte[] privateKey, string? alias = null, string? address = null)
    {
        TryAdd(new AElfAccountInfo
        {
            Alias = alias,
            Address = address
        }, privateKey);
    }
}

public class AElfAccountInfo
{
    public string? Alias { get; set; }
    public string? Address { get; set; }
}