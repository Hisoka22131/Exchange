using NBitcoin;
using NBitcoin.Altcoins;

namespace Exchange.Blockchains.Services;

public class LitecoinBlockchainService
{
    private readonly BitcoinSecret _privateKey;
    private readonly string _walletAddress;
    private readonly string _rpcUrl;

    public LitecoinBlockchainService(string rpcUrl, string privateKey)
    {
        _rpcUrl = rpcUrl;

        var litecoinNetwork = Litecoin.Instance.Mainnet;
        _privateKey = new BitcoinSecret(privateKey, litecoinNetwork);

        _walletAddress = _privateKey.GetAddress(ScriptPubKeyType.Segwit).ToString();
    }

    public string GetWalletAddress()
    {
        return _walletAddress;
    }
}