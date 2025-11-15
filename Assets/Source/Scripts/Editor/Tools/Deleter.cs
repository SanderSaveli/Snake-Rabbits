using SanderSaveli.UDK;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public static class Deleter
    {
        [MenuItem("Tools/Delete/Delete all")]
        public static void DeleteAll()
        {
            DeleteLevelProgress();
        }

        [MenuItem("Tools/Delete/Delete levels progress")]
        public static void DeleteLevelProgress()
        {
            IStorageService storage = new JsonToFileStorageService();
            storage.Save(Const.LEVEL_PROGRESS_PATH, "", (_) => { Debug.Log("Level Progress deleted!"); });
        }
    }
}
