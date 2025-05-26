using Exchange.Blockchains.Helpers;
using Exchange.Blockchains.Options;
using Microsoft.Extensions.Options;
using TronSharp;

namespace Exchange.Blockchains.Services;

public class TronBlockchainService
{
    /// <summary>
    /// USDT TRX contract address
    /// </summary>
    private const string ContractAddress = "TR7NHqjeKQxGTCi8q8ZY4pL8otSzgjLj6t";
    
    private readonly ITronClient _tronClient;
    private readonly BlockchainsOptions _blockchainsOptions;

    public TronBlockchainService(ITronClient tronClient, IOptions<BlockchainsOptions> blockchainsOptions)
    {
        _tronClient = tronClient;
        _blockchainsOptions = blockchainsOptions.Value;
    }

    public async Task TransferAsync()
    {
        try
        {
            var privateKey = TronWalletHelper.GetPrivateKeyFromSeedPhrase(_blockchainsOptions.TrustWalletSeedPhrase);
            const string fromAddress = "TKvG1E2FhYAwrKChRonCv3hk779U24GKUg";

            var to = "TNDTGoJ3dDvEmNHPCit9UUJVqFswaY7yvC";
            var amount = 10; //USDT Amount
            var contractClient = _tronClient.GetTRC20Contract();
            
            var result = await contractClient.TransferAsync(
                contractAddress: ContractAddress,
                ownerAccountAddress: fromAddress,
                ownerAccountPrivateKey: privateKey,
                toAddress: to,
                amount: amount,
                memo: string.Empty);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при отправке USDT (TRC-20): {ex.Message}");
        }
    }
}