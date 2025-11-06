using System.Drawing;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class NumberLevelGUIGroup : IConfigGUIGroup
    {
        private bool _isChangeLevelNumber =false;
        private int _originalLevelNumber;

        public void OpenNewConfig(LevelConfig levelConfig)
        {
            _originalLevelNumber = levelConfig.level_number;
        }

        public void Draw(LevelConfig levelConfig)
        {
            GUILayout.BeginHorizontal();
            if (!_isChangeLevelNumber)
            {
                GUILayout.Label("Level Number");
                GUILayout.FlexibleSpace();
                GUILayout.Label(levelConfig.level_number.ToString());
            }
            else
            {
                int number = levelConfig.level_number;
                number = EditorGUILayout.IntField("Level Number", number);
                if(number < 0)
                {
                    number = 0;
                }
                levelConfig.level_number = number;
            }
            _isChangeLevelNumber = GUILayout.Toggle(_isChangeLevelNumber, "ChangeNumber", EditorStyles.miniButton);
            GUILayout.EndHorizontal();
            if(_originalLevelNumber != levelConfig.level_number)
            {
                if (File.Exists(Const.LEVEL_FOLDER_PATH + levelConfig.level_number + ".json"))
                {
                    EditorGUILayout.HelpBox($"There is already a level with number {levelConfig.level_number}, it will be overwritten when saving!", MessageType.Warning);
                }
            }
        }
    }
}
