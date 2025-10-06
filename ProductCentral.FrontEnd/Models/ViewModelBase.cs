namespace ProductCentral.FrontEnd.Models;

/// <summary>
/// Provides a base class for view models with common error handling functionality.
/// </summary>
public abstract class ViewModelBase
{
    /// <summary>
    /// Gets or sets a value indicating whether the view model has an error.
    /// </summary>
    /// <value>
    /// <c>true</c> if the view model has an error; otherwise, <c>false</c>.
    /// </value>
    public bool HasErrror { get; set; } = false;

    /// <summary>
    /// Gets or sets the error message associated with the view model.
    /// </summary>
    /// <value>
    /// A string containing the error message, or <c>null</c> if no error message is set.
    /// </value>
    public string ErrorMessage { get; set; }
}
