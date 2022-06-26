/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, June 26, 2022.
  Released into the public domain.
*/

using UnityEditor;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public class LocalizationEditorWindow : EditorWindow
    {
        private static LocalizationEditorWindow instance;
        [MenuItem("Donut Studio/Open Localizer")]
        public static void Open()
        {
            if (instance != null)
                return;

            instance = new LocalizationEditorWindow
            {
                titleContent = new GUIContent("Localizer Window"),
                minSize = new Vector2(250, 90),
                maxSize = new Vector2(250, 90)
            };
            instance.ShowUtility();
        }

        string keyAdd = "";
        string keyRemove = "";
        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            keyAdd = EditorGUILayout.TextField(keyAdd, GUILayout.MaxWidth(250));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Add") && !string.IsNullOrWhiteSpace(keyAdd))
            {
                CSVLoader csvLoader = new CSVLoader();
                csvLoader.Add(keyAdd);

                Debug.Log($"--- Localizer: added key '{keyAdd}' ---");
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space();

            
            EditorGUILayout.BeginHorizontal();
            keyRemove = EditorGUILayout.TextField(keyRemove, GUILayout.MaxWidth(250));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Remove") && !string.IsNullOrWhiteSpace(keyRemove))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.Remove(keyRemove))
                    Debug.Log($"--- Localizer: removed key '{keyRemove}' ---");
                else
                    Debug.LogError($"--- Localizer: failed to remove key '{keyRemove}' ---");
            }
            EditorGUILayout.EndHorizontal();
        }
        public void OnDestroy()
        {
            instance = null;
        }
    }
}
    
