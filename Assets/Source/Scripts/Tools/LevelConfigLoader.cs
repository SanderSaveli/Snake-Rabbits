using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public static class LevelConfigLoader
    {
        public static List<LevelConfig> LoadAllLevel()
        {
            List<LevelConfig> levels = new List<LevelConfig>();

#if UNITY_EDITOR
            //List<string> filePaths = GetAllConfigsPaths();

            //foreach (string path in filePaths)
            //{
            //    string json = File.ReadAllText(path);
            //    LevelConfig cfg = JsonUtility.FromJson<LevelConfig>(json);
            //    if (cfg != null) levels.Add(cfg);
            //}
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Levels");

            foreach (TextAsset file in jsonFiles.OrderBy(f => ParseLevelNumber(f.name)))
            {
                LevelConfig cfg = JsonUtility.FromJson<LevelConfig>(file.text);
                if (cfg != null) levels.Add(cfg);
            }

#else
            // ---- ЗАГРУЗКА ИЗ RESOURCES ----
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Levels");

            foreach (TextAsset file in jsonFiles.OrderBy(f => ParseLevelNumber(f.name)))
            {
                LevelConfig cfg = JsonUtility.FromJson<LevelConfig>(file.text);
                if (cfg != null) levels.Add(cfg);
            }
#endif

            return levels;
        }


        public static LevelConfig LoadConfig(int number)
        {
#if UNITY_EDITOR
            //string path = $"{Const.LEVEL_FOLDER_PATH}{number}.json";

            //if (File.Exists(path))
            //{
            //    string json = File.ReadAllText(path);
            //    return JsonUtility.FromJson<LevelConfig>(json);
            //}

            //Debug.LogError($"Level file not found: {path}");
            //return null;
            TextAsset asset = Resources.Load<TextAsset>($"Levels/{number}");

            if (asset == null)
            {
                Debug.LogError($"Level {number} not found in Resources/Levels/");
                return null;
            }

            return JsonUtility.FromJson<LevelConfig>(asset.text);

#else
            TextAsset asset = Resources.Load<TextAsset>($"Levels/{number}");

            if (asset == null)
            {
                Debug.LogError($"Level {number} not found in Resources/Levels/");
                return null;
            }

            return JsonUtility.FromJson<LevelConfig>(asset.text);
#endif
        }


#if UNITY_EDITOR

        public static void SaveConfig(LevelConfig levelConfig)
        {
            if (!Directory.Exists(Const.LEVEL_FOLDER_PATH))
                Directory.CreateDirectory(Const.LEVEL_FOLDER_PATH);

            string fileName = $"{levelConfig.level_number}.json";
            string fullPath = Path.Combine(Const.LEVEL_FOLDER_PATH, fileName);

            string json = JsonUtility.ToJson(levelConfig, true);
            File.WriteAllText(fullPath, json);

            Debug.Log($"Level saved: {fullPath}");
        }

#endif

        public static List<string> GetAllConfigsPaths()
        {
#if UNITY_EDITOR
            // --- EDITOR: читаем файлы с диска ---
            //if (!Directory.Exists(Const.LEVEL_FOLDER_PATH))
            //{
            //    Debug.LogWarning($"Folder {Const.LEVEL_FOLDER_PATH} not found!");
            //    return new List<string>();
            //}

            //return Directory.GetFiles(Const.LEVEL_FOLDER_PATH, "*.json")
            //    .OrderBy(f => ParseLevelNumber(Path.GetFileNameWithoutExtension(f)))
            //    .ToList();
            TextAsset[] files = Resources.LoadAll<TextAsset>("Levels");

            return files
                .OrderBy(f => ParseLevelNumber(f.name))
                .Select(f => $"{Const.LEVEL_FOLDER_PATH}{f.name}.json")   // псевдо-путь
                .ToList();

#else
        TextAsset[] files = Resources.LoadAll<TextAsset>("Levels");

        return files
            .OrderBy(f => ParseLevelNumber(f.name))
            .Select(f => $"Levels/{f.name}")   // псевдо-путь
            .ToList();
#endif
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

        private static int ParseLevelNumber(string name)
        {
            if (int.TryParse(name, out int num))
                return num;

            return int.MaxValue;
        }
    }
}
