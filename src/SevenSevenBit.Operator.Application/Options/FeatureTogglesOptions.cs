namespace SevenSevenBit.Operator.Application.Options;

public class FeatureTogglesOptions
{
    /// <summary>
    /// The configuration key for accessing the Feature Toggles options in a configuration file.
    /// </summary>
    public const string FeatureToggles = "FeatureToggles";

    /// <summary>
    /// Gets or sets a value indicating whether the application should use mTLS for StarkEx connections.
    /// </summary>
    public bool UseMtls { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the TxId should be generated from StarkEx testing api.
    /// </summary>
    public bool GenerateTransactionIdFromStarkExApi { get; set; }
}