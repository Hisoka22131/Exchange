using NBitcoin;

namespace Exchange.Blockchains.Services;

public class BitcoinBlockchainService
{
    private readonly BitcoinSecret _privateKey;
    private readonly string _walletAddress;
    private readonly string _rpcUrl;

    public BitcoinBlockchainService(string rpcUrl, string privateKey)
    {
        _rpcUrl = rpcUrl;
        _privateKey = new BitcoinSecret(privateKey, Network.Main);

        _walletAddress = _privateKey.GetAddress(ScriptPubKeyType.Segwit).ToString();
    }

    public string GetWalletAddress()
    {
        return _walletAddress;
    }
}