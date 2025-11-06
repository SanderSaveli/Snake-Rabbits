using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class DataManager : MonoBehaviour
    {
        public List<LevelData> Levels { get; private set; }

        void Awake()
        {
            Levels = LoadAllLevel();
        }

        private List<LevelData> LoadAllLevel()
        {
            List<LevelData> levels = new List<LevelData>();
            List<string> names = Directory.GetFiles(Const.LEVEL_FOLDER_PATH, "*.json")
                .OrderBy(f =>
                {
                    string name = Path.GetFileNameWithoutExtension(f);
                    if (int.TryParse(name, out int num)) return num;
                    return int.MaxValue;
                })
                .ToList();

            foreach (string name in names)
            {
                string json = File.ReadAllText(name);
                LevelConfig levelConfig = JsonConvert.DeserializeObject<LevelConfig>(json);
                levels.Add(new LevelData(levelConfig, new LevelSaveData()));
            }

            return levels;
        }
    }
}
