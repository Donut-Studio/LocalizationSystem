/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public class LocalizationEditorWindow : EditorWindow
    {
        private static LocalizationEditorWindow instance;
        private static LocalizationSystemConfig config;
        private static TextAsset csvFile;

        [MenuItem("Donut Studio/Open Localizer")]
        public static void Open()
        {
            if (instance != null)
                return;

            instance = new LocalizationEditorWindow
            {
                titleContent = new GUIContent("Localizer"),
                minSize = new Vector2(400, 85),
                maxSize = new Vector2(400, 85)
            };
            instance.ShowUtility();
        }

        string keyValue = "";
        string languageValue = "";
        public void OnGUI()
        {
            #region Sort
            EditorGUILayout.BeginHorizontal();
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Sort"))
            {
                CSVLoader csvLoader = new CSVLoader();
                csvLoader.SortFile();
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.Space();

            #region Add/Remove Language
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Language", GUILayout.MaxWidth(75));
            languageValue = EditorGUILayout.TextField(languageValue, GUILayout.MaxWidth(175));
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Add") && !string.IsNullOrWhiteSpace(languageValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                csvLoader.AddLanguage(languageValue);

                Debug.Log($"Localizer: added language '{languageValue}'.");
            }
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Remove") && !string.IsNullOrWhiteSpace(languageValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.RemoveLanguage(languageValue))
                    Debug.Log($"Localizer: removed language '{languageValue}'.");
                else
                    Debug.LogError($"Localizer: failed to remove language '{languageValue}'.");
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.Space();

            #region Add/Remove Key
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(75));
            keyValue = EditorGUILayout.TextField(keyValue, GUILayout.MaxWidth(175));
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Add") && !string.IsNullOrWhiteSpace(keyValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                csvLoader.AddKey(keyValue);

                Debug.Log($"Localizer: added key '{keyValue}'.");
            }
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Remove") && !string.IsNullOrWhiteSpace(keyValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.RemoveKey(keyValue))
                    Debug.Log($"Localizer: removed key '{keyValue}'.");
                else
                    Debug.LogError($"Localizer: failed to remove key '{keyValue}'.");
            }
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        public void OnDestroy()
        {
            instance = null;
        }
        
        [InitializeOnLoadMethod()]
        private static void OnInitialize()
        {
            FetchConfig();
        }
        private static void FetchConfig()
        {
            while (true)
            {
                // is a config file already found
                if (config != null)
                    return;

                // get the path of the config file
                string path = GetConfigPath();
                if (path == null)
                {
                    // create the config file 
                    AssetDatabase.CreateAsset(CreateInstance(nameof(LocalizationSystemConfig)), $"Assets/Resources/{nameof(LocalizationSystemConfig)}.asset");
                    Debug.Log("Localizer: A config file for the localization system has been created. You can find it in the 'Resources' folder.");
                    continue;
                }

                // load the config file
                config = AssetDatabase.LoadAssetAtPath<LocalizationSystemConfig>(path);
                break;
            }
        }
        private static string GetConfigPath()
        {
            // search in the resources folder for the config file
            var paths = AssetDatabase.FindAssets(nameof(LocalizationSystemConfig), new string[] { "Assets/Resources" }).Select(AssetDatabase.GUIDToAssetPath).Where(c => c.EndsWith(".asset"));
            if (paths.Count() > 1)
                Debug.LogError("Localizer: Multiple config files found!");
            return paths.FirstOrDefault();
        }
    }
}