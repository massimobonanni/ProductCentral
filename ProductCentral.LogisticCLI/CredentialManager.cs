using System.Text;
using System.Text.Json;

namespace ProductCentral.LogisticCLI;

public class CredentialManager
{
    private readonly string _filePath;
    private readonly byte[] _encryptionKey;
    private readonly bool _encryptCredentials;
    private readonly TimeSpan _validityDuration = TimeSpan.FromMinutes(5);

    public CredentialManager(string filePath, string encryptionKey, bool encryptCredentials = true, TimeSpan? validityDuration = null)
    {
        _filePath = filePath;
        _encryptionKey = Encoding.UTF8.GetBytes(encryptionKey);
        _encryptCredentials = encryptCredentials;
        if (validityDuration.HasValue)
        {
            _validityDuration = validityDuration.Value;
        }
    }

    public void SetupCredentials(Credentials credentials)
    {
        credentials.Timestamp = DateTime.Now;

        var json = JsonSerializer.Serialize(credentials);
        var dataToWrite = _encryptCredentials ? json.Encrypt(_encryptionKey) : json;

        File.WriteAllText(_filePath, dataToWrite);
    }

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

public class Credentials
{
    public string Url { get; set; }
    public string Key { get; set; }
    public DateTime Timestamp { get; set; }
}
