using Exchange.Blockchains.Helpers;
using Exchange.Blockchains.Options;
using Exchange.Blockchains.Services;
using Exchange.Blockchains.Services.Test;
using Exchange.Common;
using Exchange.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TronSharp;

namespace Exchange.Blockchains;

public static class ServiceCollectionExtensions
{
    public static void AddBlockchainServices(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(BlockchainsOptions.SectionName);

        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));

        var options = configurationSection.Get<BlockchainsOptions>()!;

        services
            .AddOptions<BlockchainsOptions>()
            .Bind(configurationSection);

        services
            .AddNethereumBlockchain(options)
            .AddBitcoinBlockchain(options)
            .AddLitecoinBlockchain(options)
            .AddTronBlockchain(options)
            ;
    }

    private static IServiceCollection AddNethereumBlockchain(this IServiceCollection services,
        BlockchainsOptions options)
    {
        var nethereumPrivateKey = NethereumWalletHelper.GetPrivateKeyFromSeedPhrase(options.TrustWalletSeedPhrase);

        services.AddSingleton(_ =>
        {
            var rpcUrl = $"{options.Nethereum.RpcUrl}/{options.Nethereum.ApiKey}";
            return new NethereumBlockchainService(rpcUrl, nethereumPrivateKey);
        });

        return services;
    }

    private static IServiceCollection AddBitcoinBlockchain(this IServiceCollection services, BlockchainsOptions options)
    {
        var btcPrivateKey = BitcoinWalletHelper.GetPrivateKeyFromSeedPhrase(options.TrustWalletSeedPhrase);

        services.AddSingleton(_ =>
        {
            var rpcUrl = options.Bitcoin.RpcUrl;
            return new BitcoinBlockchainService(rpcUrl, btcPrivateKey);
        });

        return services;
    }

    private static IServiceCollection AddLitecoinBlockchain(this IServiceCollection services,
        BlockchainsOptions options)
    {
        var ltcPrivateKey = LitecoinWalletHelper.GetPrivateKeyFromSeedPhrase(options.TrustWalletSeedPhrase);

        services.AddSingleton(_ =>
        {
            var rpcUrl = options.Litecoin.RpcUrl;
            return new LitecoinBlockchainService(rpcUrl, ltcPrivateKey);
        });

        return services;
    }

    private static IServiceCollection AddTronBlockchain(this IServiceCollection services, BlockchainsOptions options)
    {
        services.AddSingleton<TronBlockchainService>();

        services.AddTronSharp(x =>
        {
            x.Network = TronNetwork.MainNet;
            x.Channel = new GrpcChannelOption { Host = "grpc.trongrid.io", Port = 50051 };
            x.SolidityChannel = new GrpcChannelOption { Host = "grpc.trongrid.io", Port = 50052 };
            x.FreeApiKey = options.Tron.ApiKey;
        });

        return services;
    }
}