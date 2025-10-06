namespace ProductCentral.FrontEnd.Models;

/// <summary>
/// Represents the view model for error pages, containing information about request identification and display logic.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the request that caused the error.
    /// </summary>
    /// <value>
    /// The request ID string, or null if no request ID is available.
    /// </value>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the request ID should be displayed to the user.
    /// </summary>
    /// <value>
    /// true if the RequestId is not null or empty; otherwise, false.
    /// </value>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
