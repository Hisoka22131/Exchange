using NBitcoin;
using NBitcoin.Altcoins;

namespace Exchange.Blockchains.Helpers;

public static class LitecoinWalletHelper
{
    public static string GetPrivateKeyFromSeedPhrase(string seedPhrase)
    {
        try
        {
            var mnemonic = new Mnemonic(seedPhrase, Wordlist.English);

            var masterKey = mnemonic.DeriveExtKey();

            const string path = "m/84'/2'/0'/0/0";
            var key = masterKey.Derive(new KeyPath(path));

            return key.PrivateKey.GetWif(Litecoin.Instance.Mainnet).ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обработке seed phrase: {ex.Message}");
        }
    }
}