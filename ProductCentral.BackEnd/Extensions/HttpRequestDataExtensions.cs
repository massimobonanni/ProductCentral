using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Functions.Worker.Http;

/// <summary>
/// Extension methods for HttpRequestData to create various types of HttpResponseData.
/// </summary>
public static class HttpRequestDataExtensions
{
    /// <summary>
    /// Creates a BadRequest (400) response with a specified message.
    /// </summary>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HttpResponseData.</returns>
    public static Task<HttpResponseData> CreateBadRequestResponseAsync(this HttpRequestData req, string message)
    {
        return req.CreateStringResponseAsync(HttpStatusCode.BadRequest, message);
    }

    /// <summary>
    /// Creates a NotFound (404) response with a specified message.
    /// </summary>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HttpResponseData.</returns>
    public static Task<HttpResponseData> CreateNotFoundResponseAsync(this HttpRequestData req, string message)
    {
        return req.CreateStringResponseAsync(HttpStatusCode.NotFound, message);
    }

    /// <summary>
    /// Creates an OK (200) response with specified content.
    /// </summary>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <param name="content">The content to include in the response.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HttpResponseData.</returns>
    public static Task<HttpResponseData> CreateOkResponseAsync(this HttpRequestData req, object content)
    {
        return req.CreateResponseAsync(HttpStatusCode.OK, content);
    }

    /// <summary>
    /// Creates a response with a specified status code and message.
    /// </summary>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <param name="statusCode">The HTTP status code for the response.</param>
    /// <param name="message">The message to include in the response.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HttpResponseData.</returns>
    public static async Task<HttpResponseData> CreateStringResponseAsync(this HttpRequestData req, HttpStatusCode statusCode, string message)
    {
        var response = req.CreateResponse(statusCode);
        await response.WriteStringAsync(message);
        return response;
    }

    /// <summary>
    /// Creates a response with a specified status code.
    /// </summary>
    /// <param name="req"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static Task<HttpResponseData> CreateResponseAsync(this HttpRequestData req, HttpStatusCode statusCode)
    {
        var response = req.CreateResponse(statusCode);
        return Task.FromResult(response);
    }

    /// <summary>
    /// Creates a response with a specified status code and content.
    /// </summary>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <param name="statusCode">The HTTP status code for the response.</param>
    /// <param name="content">The content to include in the response.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HttpResponseData.</returns>
    public static async Task<HttpResponseData> CreateResponseAsync(this HttpRequestData req, HttpStatusCode statusCode, object content)
    {
        var response = req.CreateResponse(statusCode);
        await response.WriteAsJsonAsync(content);
        return response;
    }

    /// <summary>
    /// Reads the request body and deserializes it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the request body to.</typeparam>
    /// <param name="req">The HttpRequestData instance.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized object of type T.</returns>
    public static async Task<T?> GetRequestBodyAsync<T>(this HttpRequestData req)
    {
        string? requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        return JsonSerializer.Deserialize<T>(requestBody,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
    }
}
