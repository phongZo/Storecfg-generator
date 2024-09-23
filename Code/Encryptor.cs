// Decompiled with JetBrains decompiler
// Type: StorecfgGenerator.Encryptor
// Assembly: StorecfgGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0723BBEC-1C2F-455C-81B7-FC369DB2048E
// Assembly location: C:\AgileMomentum\AgileMark\source\StoreCfgGenerator\StorecfgGenerator.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace StorecfgGenerator
{
  internal static class Encryptor
  {
    private const string EncryptionKey = "sklGkwfiszlREs1d0hSQO9cJLb6FVT59rrzWOLYoZIISNP0kHoZVdZyhj4XHcxUW";

    private static string Encrypt(byte[] clearBytes)
    {
      string str = "";
      using (Aes aes = Aes.Create())
      {
        Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("sklGkwfiszlREs1d0hSQO9cJLb6FVT59rrzWOLYoZIISNP0kHoZVdZyhj4XHcxUW", new byte[13]
        {
          (byte) 73,
          (byte) 118,
          (byte) 97,
          (byte) 110,
          (byte) 32,
          (byte) 77,
          (byte) 101,
          (byte) 100,
          (byte) 118,
          (byte) 101,
          (byte) 100,
          (byte) 101,
          (byte) 118
        });
        aes.Key = rfc2898DeriveBytes.GetBytes(32);
        aes.IV = rfc2898DeriveBytes.GetBytes(16);
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(clearBytes, 0, clearBytes.Length);
            cryptoStream.Close();
          }
          str = Convert.ToBase64String(memoryStream.ToArray());
        }
      }
      return str;
    }

    private static string Decrypt(string cipherText)
    {
      try
      {
        cipherText = cipherText.Replace(" ", "+");
        byte[] buffer = Convert.FromBase64String(cipherText);
        using (Aes aes = Aes.Create())
        {
          Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("sklGkwfiszlREs1d0hSQO9cJLb6FVT59rrzWOLYoZIISNP0kHoZVdZyhj4XHcxUW", new byte[13]
          {
            (byte) 73,
            (byte) 118,
            (byte) 97,
            (byte) 110,
            (byte) 32,
            (byte) 77,
            (byte) 101,
            (byte) 100,
            (byte) 118,
            (byte) 101,
            (byte) 100,
            (byte) 101,
            (byte) 118
          });
          aes.Key = rfc2898DeriveBytes.GetBytes(32);
          aes.IV = rfc2898DeriveBytes.GetBytes(16);
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
              cryptoStream.Write(buffer, 0, buffer.Length);
              cryptoStream.Close();
            }
            cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
          }
        }
        return cipherText;
      }
      catch (Exception ex)
      {
        Console.WriteLine("Decryption failed");
        Console.WriteLine("Message: " + ex?.ToString());
        return "";
      }
    }

    private static byte[] DecryptToByte(string cipherText)
    {
      try
      {
        cipherText = cipherText.Replace(" ", "+");
        byte[] buffer = Convert.FromBase64String(cipherText);
        using (Aes aes = Aes.Create())
        {
          Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes("sklGkwfiszlREs1d0hSQO9cJLb6FVT59rrzWOLYoZIISNP0kHoZVdZyhj4XHcxUW", new byte[13]
          {
            (byte) 73,
            (byte) 118,
            (byte) 97,
            (byte) 110,
            (byte) 32,
            (byte) 77,
            (byte) 101,
            (byte) 100,
            (byte) 118,
            (byte) 101,
            (byte) 100,
            (byte) 101,
            (byte) 118
          });
          aes.Key = rfc2898DeriveBytes.GetBytes(32);
          aes.IV = rfc2898DeriveBytes.GetBytes(16);
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
              cryptoStream.Write(buffer, 0, buffer.Length);
              cryptoStream.Close();
            }
            return memoryStream.ToArray();
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Decryption failed");
        Console.WriteLine("Message: " + ex?.ToString());
        return (byte[]) null;
      }
    }

    public static string ENCODE(byte[] byteData) => Encryptor.Encrypt(byteData);

    public static string DECODE(string code) => Encryptor.Decrypt(code);

    public static byte[] DecodeToByte(string code) => Encryptor.DecryptToByte(code);
  }
}
