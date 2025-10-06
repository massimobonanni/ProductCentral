using System.Text;
using System.Text.Json;

namespace ProductCentral.LogisticCLI;

/// <summary>
/// Manages encrypted credential storage and retrieval with time-based validity checking.
/// </summary>
public class CredentialManager
{
    private readonly string _filePath;
    private readonly byte[] _encryptionKey;
    private readonly bool _encryptCredentials;
    private readonly TimeSpan _validityDuration = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialManager"/> class.
    /// </summary>
    /// <param name="filePath">The file path where credentials will be stored.</param>
    /// <param name="encryptionKey">The encryption key used to encrypt/decrypt credentials.</param>
    /// <param name="encryptCredentials">Whether to encrypt credentials when storing them. Default is true.</param>
    /// <param name="validityDuration">The duration for which credentials remain valid. Default is 5 minutes.</param>
    public CredentialManager(string filePath, string encryptionKey, bool encryptCredentials = true, TimeSpan? validityDuration = null)
    {
        _filePath = filePath;
        if (!string.IsNullOrEmpty(encryptionKey))
        {
            _encryptionKey = Encoding.UTF8.GetBytes(encryptionKey);
        }
        _encryptCredentials = encryptCredentials;
        if (validityDuration.HasValue)
        {
            _validityDuration = validityDuration.Value;
        }
    }

    /// <summary>
    /// Stores the provided credentials to the file system with a current timestamp.
    /// </summary>
    /// <param name="credentials">The credentials to store.</param>
    public void SetupCredentials(Credentials credentials)
    {
        credentials.Timestamp = DateTime.Now;

        var json = JsonSerializer.Serialize(credentials);
        var dataToWrite = _encryptCredentials ? json.Encrypt(_encryptionKey) : json;

        File.WriteAllText(_filePath, dataToWrite);
    }

    /// <summary>
    /// Checks whether the stored credentials are valid based on their timestamp and the configured validity duration.
    /// </summary>
    /// <returns>True if credentials exist and are within the validity period; otherwise, false.</returns>
    public bool AreCredentialsValid()
    {
        if (!File.Exists(_filePath))
        {
            return false;
        }

        var dataFromFile = File.ReadAllText(_filePath);
        var json = _encryptCredentials ? dataFromFile.Decrypt(_encryptionKey) : dataFromFile;
        var credentials = JsonSerializer.Deserialize<Credentials>(json);

        return credentials.Timestamp.Add(_validityDuration) >= DateTime.Now;
    }

    /// <summary>
    /// Retrieves the stored credentials if they are valid and updates their timestamp.
    /// </summary>
    /// <returns>The credentials if valid; otherwise, null.</returns>
    public Credentials GetCredentials()
    {
        if (!AreCredentialsValid())
        {
            return null;
        }

        var dataFromFile = File.ReadAllText(_filePath);
        var json = _encryptCredentials ? dataFromFile.Decrypt(_encryptionKey) : dataFromFile;
        var credentials = JsonSerializer.Deserialize<Credentials>(json);
        SetupCredentials(credentials); // Update the timestamp

        return credentials;
    }
}

/// <summary>
/// Represents connection credentials with timestamp information.
/// </summary>
public class Credentials
{
    /// <summary>
    /// Gets or sets the connection string for the service.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the name of the topic or queue.
    /// </summary>
    public string TopicOrQueueName { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the credentials were last updated.
    /// </summary>
    public DateTime Timestamp { get; set; }
}
