using Exchange.Blockchains.Helpers;
using TronSharp;

namespace Exchange.Blockchains.Services.Test;

public class TronBlockchainTestService
{
    /// <summary>
    /// USDT TRX contract address
    /// </summary>
    private const string ContractAddress = "TXYZopYRdj2D9XRtbG411XZZ3kM5VkAeBf";
    
    private readonly ITronClient _tronClient;

    public TronBlockchainTestService(ITronClient tronClient)
    {
        _tronClient = tronClient;
    }

    public async Task TransferAsync()
    {
        try
        {
            var privateKey = TronWalletHelper.GetPrivateKeyFromSeedPhrase("disagree lemon safe whip whale day battle stove oak poem husband cage");
            const string fromAddress = "TU7oPeAoMpdSctzkNCn8QcLvYnGfmvaJBt";

            var to = "TGehVcNhud84JDCGrNHKVz9jEAVKUpbuiv";
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