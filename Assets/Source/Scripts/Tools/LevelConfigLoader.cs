using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public static class LevelConfigLoader
    {
        public static List<LevelConfig> LoadAllLevel()
        {
            List<LevelConfig> levels = new List<LevelConfig>();
            List<string> names = GetAllConfigsPaths();
            names = names.OrderBy(f =>
                {
                    string name = Path.GetFileNameWithoutExtension(f);
                    if (int.TryParse(name, out int num)) return num;
                    return int.MaxValue;
                })
                .ToList();

            foreach (string name in names)
            {
                levels.Add(LoadDirectly(name));
            }

            return levels;
        }

        public static List<string> GetAllConfigsPaths()
        {
            if (!Directory.Exists(Const.LEVEL_PROGRESS_PATH))
            {
                Debug.LogWarning($"Folder {Const.LEVEL_PROGRESS_PATH} not found!");
                return new List<string>();
            }

            return Directory.GetFiles(Const.LEVEL_PROGRESS_PATH, "*.json")
                .OrderBy(f =>
                {
                    string name = Path.GetFileNameWithoutExtension(f);
                    if (int.TryParse(name, out int num)) return num;
                    return int.MaxValue;
                })
                .ToList();
        }

        public static void SaveConfig(LevelConfig levelConfig)
        {
            string fileName = $"{levelConfig.level_number}.json";
            string fullPath = Path.Combine(Const.LEVEL_FOLDER_PATH, fileName);
            string json = JsonUtility.ToJson(levelConfig, true);
            File.WriteAllText(fullPath, json);
        }

        public static LevelConfig LoadConfig(int number)
        {
            string path = $"{Const.LEVEL_FOLDER_PATH}{number}.json";
            return LoadDirectly(path);
        }

        public static LevelConfig LoadDirectly(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<LevelConfig>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Ошибка при загрузке LevelConfig: " + e.Message);
                return null;
            }
        }
    }
}
