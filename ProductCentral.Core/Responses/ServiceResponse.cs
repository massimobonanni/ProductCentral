namespace ProductCentral.Core.Responses;

/// <summary>
/// Represents a service response with a success status and an error message.
/// </summary>
public class ServiceResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation was not successful.
    /// </summary>
    public string ErrorMessage { get; set; }
}

/// <summary>
/// Represents a service response with a success status, an error message, and a result.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public class ServiceResponse<T> : ServiceResponse
{
    /// <summary>
    /// Gets or sets the result of the operation.
    /// </summary>
    public T Result { get; set; }
}

