using Newtonsoft.Json;
using SanderSaveli.UDK;
using System;
using System.IO;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class ResourceStorageService : IStorageService
    {
        public void Load<T>(string key, Action<T> callback)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Load failed: key is null or empty.");
                callback?.Invoke(default);
                return;
            }

            try
            {
                UnityEngine.Object obj = Resources.Load(key);

                if (obj == null)
                {
                    Debug.LogWarning($"Resource not found: {key}");
                    callback?.Invoke(default);
                    return;
                }

                if (obj is T casted)
                {
                    callback?.Invoke(casted);
                }
                else
                {
                    Debug.LogError($"Resource '{key}' cannot be cast to {typeof(T)}");
                    callback?.Invoke(default);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading resource '{key}': {e}");
                callback?.Invoke(default);
            }
        }

        public void Save(string key, object data, Action<bool> callback = null)
        {
#if !UNITY_EDITOR
            Debug.LogError("Can't save to resources in build");
            callback?.Invoke(false);
            return;
#endif
            string path = Path.Combine("Assets/Resources/", key + ".json");

            string jsonFile = JsonConvert.SerializeObject(data, Formatting.Indented);

            try
            {
                string tempDirectory = Path.GetDirectoryName(path);
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }

                using (var fileStream = new StreamWriter(path))
                {
                    fileStream.Write(jsonFile);
                }
                callback?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in data save: {ex.Message}");
                callback?.Invoke(false);
            }
        }
    }
}
