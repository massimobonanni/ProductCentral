using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ProductCentral.LogisticCLI;

public class CredentialManager
{
    private readonly string _filePath;
    private readonly byte[] _encryptionKey;
    private readonly bool _encryptCredentials;

    public CredentialManager(string filePath, string encryptionKey, bool encryptCredentials = true)
    {
        _filePath = filePath;
        _encryptionKey = Encoding.UTF8.GetBytes(encryptionKey);
        _encryptCredentials = encryptCredentials;
    }

    public void SetupCredentials(Credentials credentials)
    {
        credentials.Timestamp = DateTime.UtcNow;

        var json = JsonSerializer.Serialize(credentials);
        var dataToWrite = _encryptCredentials ? json.Encrypt(_encryptionKey) : json;

        File.WriteAllText(_filePath, dataToWrite);
    }

    public bool AreCredentialsValid(DateTime validTimestamp)
    {
        if (!File.Exists(_filePath))
        {
            return false;
        }

        var dataFromFile = File.ReadAllText(_filePath);
        var json = _encryptCredentials ? dataFromFile.Decrypt(_encryptionKey) : dataFromFile;
        var credentials = JsonSerializer.Deserialize<Credentials>(json);

        return credentials.Timestamp >= validTimestamp;
    }

    public Credentials GetCredentials()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException("Credentials file not found.");
        }

        var dataFromFile = File.ReadAllText(_filePath);
        var json = _encryptCredentials ? dataFromFile.Decrypt(_encryptionKey) : dataFromFile;
        return JsonSerializer.Deserialize<Credentials>(json);
    }
}

public class Credentials
{
    public string Url { get; set; }
    public string Key { get; set; }
    public DateTime Timestamp { get; set; }
}
