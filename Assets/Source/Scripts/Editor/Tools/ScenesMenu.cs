using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ScenesMenuWindow : EditorWindow
{
    public static void ShowMenu()
    {
        var window = CreateInstance<ScenesMenuWindow>();
        window.ShowPopup();
    }

    private void OnGUI()
    {
        // создаём меню
        GenericMenu menu = new GenericMenu();
        var scenes = EditorBuildSettings.scenes;

        for (int i = 0; i < scenes.Length; i++)
        {
            string path = scenes[i].path;
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            menu.AddItem(new GUIContent(name), false, () =>
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(path);
                }
            });
        }

        // открываем под курсором
        Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        menu.DropDown(new Rect(mousePos, Vector2.zero));

        Close();
    }
}

public static class ScenesMenu
{
    [MenuItem("Scenes/Open Scene...")]
    private static void OpenScenes()
    {
        ScenesMenuWindow.ShowMenu();
    }
}
