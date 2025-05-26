namespace Exchange.Blockchains.Options;

public sealed record BlockchainsOptions
{
    public const string SectionName = "Blockchain";
    
    public required string TrustWalletSeedPhrase { get; init; }
    
    public required BlockchainOptions Nethereum { get; init; }
    
    public required BlockchainOptions Bitcoin { get; init; }
    
    public required BlockchainOptions Litecoin { get; init; }
    
    public required BlockchainOptions Tron { get; init; }
}

public sealed record BlockchainOptions
{
    public required string RpcUrl { get; init; }
    public required string ApiKey { get; init; }
}