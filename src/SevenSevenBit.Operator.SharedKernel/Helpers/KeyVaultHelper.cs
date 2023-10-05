namespace SevenSevenBit.Operator.SharedKernel.Helpers;

public static class KeyVaultHelper
{
    public static string FormatCertificateKey(string key)
    {
        // AKV doesn't support dots in certificate keys, so we replace them with dashes. Ideally we add here more akv key rules
        return key.Replace('.', '-');
    }

    public static string FormatSecretKey(string keyPrefix, string keySuffix)
    {
        return $"{keyPrefix}-{keySuffix}";
    }
}