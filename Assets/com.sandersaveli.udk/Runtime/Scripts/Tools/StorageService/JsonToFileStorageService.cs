using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace SanderSaveli.UDK
{
    public class JsonToFileStorageService : IStorageService
    {
        public void Save(string key, object data, Action<bool> callback = null)
        {
            string path = BuildPath(key);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string jsonFile = JsonConvert.SerializeObject(data);

            using (var fileStream = new StreamWriter(path))
            {
                fileStream.Write(jsonFile);
            }
            callback?.Invoke(true);
        }

        public void Load<T>(string key, Action<T> callback)
        {
            string path = BuildPath(key);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(path))
            {
                Debug.Log("File does not exist, creating empty file");
                File.Create(path).Dispose();
                callback.Invoke(default);
                return;
            }
            T data;
            using (var fileStream = new StreamReader(path))
            {
                string jsonFile = fileStream.ReadToEnd();
                data = JsonConvert.DeserializeObject<T>(jsonFile);
            }
            callback.Invoke(data);
        }

        private string BuildPath(string key) =>
             Path.Combine(Application.persistentDataPath, key);
    }
}
