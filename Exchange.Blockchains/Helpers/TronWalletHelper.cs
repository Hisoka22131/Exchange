using NBitcoin;

namespace Exchange.Blockchains.Helpers;

public static class TronWalletHelper
{
    public static string GetPrivateKeyFromSeedPhrase(string seedPhrase)
    {
        try
        {
            var mnemonic = new Mnemonic(seedPhrase, Wordlist.English);
            var masterKey = mnemonic.DeriveExtKey();

            const string keyPath = "m/44'/195'/0'/0/0";
            var derivedKey = masterKey.Derive(new KeyPath(keyPath));

            return derivedKey.PrivateKey.ToHex();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обработке seed phrase: {ex.Message}");
        }
    }
}