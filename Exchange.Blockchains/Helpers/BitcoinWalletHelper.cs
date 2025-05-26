using NBitcoin;

namespace Exchange.Blockchains.Helpers;

public static class BitcoinWalletHelper
{
    public static string GetPrivateKeyFromSeedPhrase(string seedPhrase)
    {
        try
        {
            var mnemonic = new Mnemonic(seedPhrase, Wordlist.English);

            var masterKey = mnemonic.DeriveExtKey();

            const string path = "m/84'/0'/0'/0/0";
            var key = masterKey.Derive(new KeyPath(path));

            return key.PrivateKey.GetWif(Network.Main).ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обработке seed phrase: {ex.Message}");
        }
    }
}