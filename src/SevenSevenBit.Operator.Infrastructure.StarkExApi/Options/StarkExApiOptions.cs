namespace SevenSevenBit.Operator.Infrastructure.StarkExApi.Options;

public class StarkExApiOptions
{
    public const string StarkExApi = "Services:StarkExApi";

    public int MinValidiumTreeHeight { get; set; }

    public int MaxValidiumTreeHeight { get; set; }

    public int MinZkRollupTreeHeight { get; set; }

    public int MaxZkRollupTreeHeight { get; set; }

    public IEnumerable<string> SupportedVersions { get; set; }
}