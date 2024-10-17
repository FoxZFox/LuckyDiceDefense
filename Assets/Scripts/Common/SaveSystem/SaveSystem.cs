using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveSystem
{
    private readonly string defaultKey = "*6iw2@647@n^e7+9fn#dw!v$v#wavk+atcdyp121muo#ah3zm4";
    private readonly string fileName = "kPq5rqmFi5LwxdoA";
    public void Save(SaveData data, string key = null)
    {
        key = key ?? defaultKey;
        string jsonData = JsonUtility.ToJson(data, true);
        string encryption = Encrypt(jsonData, key);
        try
        {
            File.WriteAllText(Application.persistentDataPath + "/" + fileName + ".json", encryption);
            Debug.Log("Save");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }
    }

    public SaveData Load(string key = null)
    {
        key = key ?? defaultKey;
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        try
        {
            if (File.Exists(filePath))
            {
                string encryptionData = File.ReadAllText(filePath);
                string decrypData = Decrypt(encryptionData, key);
                return JsonUtility.FromJson<SaveData>(decrypData);
            }
            else
            {
                Debug.Log("New Game");
                var data = new SaveData();
                data.GemData = 250;
                return data;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return new SaveData();
        }
    }
#if UNITY_EDITOR
    public void DeleteSave()
    {
        string filePath = Application.persistentDataPath + "/" + fileName + ".json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file deleted");
        }
        else
        {
            Debug.Log("Save file not found");
        }
    }
#endif
    private string Encrypt(string plainText, string key)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        aes.IV = new byte[16];
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using MemoryStream ms = new();
        using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter sw = new(cs);
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());


    }

    private string Decrypt(string cipherText, string key)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
        aes.IV = new byte[16];

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using MemoryStream ms = new(Convert.FromBase64String(cipherText));
        using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
        using StreamReader sr = new(cs);
        return sr.ReadToEnd();
    }
}
