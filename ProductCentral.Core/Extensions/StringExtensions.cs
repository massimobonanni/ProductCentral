using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace System;

/// <summary>
/// Provides extension methods for string encryption and decryption operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Encrypts a plain text string using AES encryption with the provided key.
    /// </summary>
    /// <param name="plainText">The plain text string to encrypt.</param>
    /// <param name="key">The encryption key (must be valid AES key length: 128, 192, or 256 bits).</param>
    /// <returns>A Base64-encoded string containing the encrypted data with IV prepended.</returns>
    /// <exception cref="ArgumentNullException">Thrown when plainText or key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when key length is invalid for AES encryption.</exception>
    public static string Encrypt(this string plainText, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        var iv = aes.IV;
        var encrypted = ms.ToArray();

        var result = new byte[iv.Length + encrypted.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// Decrypts a Base64-encoded encrypted string using AES decryption with the provided key.
    /// </summary>
    /// <param name="cipherText">The Base64-encoded encrypted string containing IV and cipher data.</param>
    /// <param name="key">The decryption key (must match the key used for encryption).</param>
    /// <returns>The decrypted plain text string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when cipherText or key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when key length is invalid for AES decryption.</exception>
    /// <exception cref="FormatException">Thrown when cipherText is not a valid Base64 string.</exception>
    /// <exception cref="CryptographicException">Thrown when decryption fails due to invalid data or key.</exception>
    public static string Decrypt(this string cipherText, byte[] key)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = key;

        var iv = new byte[aes.BlockSize / 8];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}
