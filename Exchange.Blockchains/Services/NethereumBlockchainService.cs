using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Exchange.Blockchains.Services;

public class NethereumBlockchainService
{
    private readonly Web3 _web3; 
    private readonly string _walletAddress;

    public NethereumBlockchainService(string rpcUrl, string privateKey)
    {
        var account = new Account(privateKey);
        _web3 = new Web3(account, rpcUrl);
        _walletAddress = account.Address;
    }

    public string GetWalletAddress()
    {
        return _walletAddress;
    }
}