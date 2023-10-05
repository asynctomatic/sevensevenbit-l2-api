namespace SevenSevenBit.Operator.Web.Options.Documentation;

/// <summary>
/// Represents configuration options for generating documentation.
/// </summary>
public class DocumentationOptions
{
    /// <summary>
    /// The configuration key for accessing the documentation options in a configuration file.
    /// </summary>
    public const string Documentation = "Documentation";

    /// <summary>
    /// Gets or sets the title of the documentation.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the documentation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the URL for the terms of service of the platform.
    /// </summary>
    public string TermsOfService { get; set; }

    /// <summary>
    /// Gets or sets the URL for the license of the platform.
    /// </summary>
    public string License { get; set; }

    /// <summary>
    /// Gets or sets the URL of the logo for the documentation page.
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text for the logo of the documentation page.
    /// </summary>
    public string LogoTooltip { get; set; }
}