using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class StarLevelConfigGUIGroup : IConfigGUIGroup
    {
        private readonly GUIStyle _centeredLabel = new(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleCenter
        };

        public void Draw(LevelConfig levelConfig)
        {
            GUILayout.Label("Stars", _centeredLabel);
            GUILayout.BeginHorizontal();

            for (int i = 1; i <= 3; i++)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(i.ToString());
                if (i == 1)
                    levelConfig.score_for_first_star = Mathf.Max(0, EditorGUILayout.IntField(levelConfig.score_for_first_star));
                if (i == 2)
                    levelConfig.score_for_second_star = Mathf.Max(0, EditorGUILayout.IntField(levelConfig.score_for_second_star));
                if (i == 3)
                    levelConfig.score_for_third_star = Mathf.Max(0, EditorGUILayout.IntField(levelConfig.score_for_third_star));
                GUILayout.EndVertical();
                GUILayout.Space(15);
            }
            GUILayout.EndHorizontal();
            DrawErrors(levelConfig);
        }

        public void OpenNewConfig(LevelConfig levelConfig)
        {

        }

        private void DrawErrors(LevelConfig levelConfig)
        {
            if(levelConfig.score_for_first_star == 0)
            {
                EditorGUILayout.HelpBox("Score for star 1 == 0", MessageType.Warning);
            }
            if (levelConfig.score_for_second_star == 0)
            {
                EditorGUILayout.HelpBox("Score for star 2 == 0", MessageType.Warning);
            }
            if (levelConfig.score_for_third_star == 0)
            {
                EditorGUILayout.HelpBox("Score for star 3 == 0", MessageType.Warning);
            }
            if (levelConfig.score_for_second_star <= levelConfig.score_for_first_star)
            {
                EditorGUILayout.HelpBox("Score for 2 stars eqals or less then for 1 star", MessageType.Error);
            }
            if (levelConfig.score_for_third_star <= levelConfig.score_for_first_star)
            {
                EditorGUILayout.HelpBox("Score for 3 stars eqals or less then for 2 stars", MessageType.Error);
            }
        }
    }
}
