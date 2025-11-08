using Newtonsoft.Json;
using SanderSaveli.Snake;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelOrderWindow : EditorWindow
{
    private string _levelsFolderPath => Const.LEVEL_FOLDER_PATH;
    private Vector2 _scrollPos;
    private List<string> levelFiles = new List<string>();
    private int dragFrom = -1;

    [MenuItem("Tools/Snake/LevelOrder")]
    public static void ShowWindow()
    {
        GetWindow<LevelOrderWindow>("Level Editor");
    }

    private void OnEnable()
    {
        levelFiles = LevelConfigLoader.GetAllConfigsPaths();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Levels:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (levelFiles.Count == 0)
        {
            EditorGUILayout.HelpBox($"There is no JSON levels in folder: {_levelsFolderPath}", MessageType.Info);
            return;
        }

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        for (int i = 0; i < levelFiles.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");

            Rect rect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));
            GUI.Label(rect, $"{i + 1}. {Path.GetFileName(levelFiles[i])}");

            // Обработка двойного клика
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.clickCount == 2 && rect.Contains(e.mousePosition))
            {
                // Двойной клик по элементу
                LevelConfigWindow.OpenWithLevel(levelFiles[i]);
                e.Use(); // чтобы событие не использовалось дальше
            }

            // Кнопки вверх/вниз
            if (GUILayout.Button("↑", GUILayout.Width(25)) && i > 0)
                Swap(i, i - 1);
            if (GUILayout.Button("↓", GUILayout.Width(25)) && i < levelFiles.Count - 1)
                Swap(i, i + 1);

            // Удаление
            if (GUILayout.Button("Delete", GUILayout.Width(30)))
            {
                string file = levelFiles[i];
                if (EditorUtility.DisplayDialog("Delete Level?", $"Do you really want to delete {Path.GetFileName(file)}?", "Delete", "Cancel"))
                {
                    File.Delete(file);
                    AssetDatabase.Refresh();
                    levelFiles = LevelConfigLoader.GetAllConfigsPaths();
                    return;
                }
            }

            EditorGUILayout.EndHorizontal();
        }


        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        GUI.backgroundColor = Color.green;

        if (GUILayout.Button("Save and rename", GUILayout.Height(30)))
        {
            SaveOrder();
        }

        GUI.backgroundColor = Color.white;
    }

    private void Swap(int i, int j)
    {
        string temp = levelFiles[i];
        levelFiles[i] = levelFiles[j];
        levelFiles[j] = temp;
    }

    private void SaveOrder()
    {
        for (int i = 0; i < levelFiles.Count; i++)
        {
            string oldPath = levelFiles[i];
            string tempPath = Path.Combine(_levelsFolderPath, $"temp_{i + 1}.json");
            File.Move(oldPath, tempPath);
            levelFiles[i] = tempPath;
        }

        for (int i = 0; i < levelFiles.Count; i++)
        {
            string tempPath = levelFiles[i];
            string finalPath = Path.Combine(_levelsFolderPath, $"{i + 1}.json");

            string json = File.ReadAllText(tempPath);
            LevelConfig level = JsonConvert.DeserializeObject<LevelConfig>(json);
            level.level_number = i + 1;
            string newJson = JsonConvert.SerializeObject(level);
            File.WriteAllText(tempPath, newJson);


            File.Move(tempPath, finalPath);
            levelFiles[i] = finalPath;
        }

        AssetDatabase.Refresh();
        levelFiles = LevelConfigLoader.GetAllConfigsPaths();
        EditorUtility.DisplayDialog("Done", "Levels successfully renamed!", "OK");
    }
}
