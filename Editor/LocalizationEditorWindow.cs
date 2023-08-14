/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DonutStudio.LocalizationSystem
{
    public class LocalizationEditorWindow : EditorWindow
    {
        private static LocalizationEditorWindow _instance;
        private static LocalizationSystemConfig _config;
        private static TextAsset _csvFile;

        [MenuItem("Donut Studio/Open Localizer")]
        public static void Open()
        {
            if (_instance != null)
                return;

            _instance = new LocalizationEditorWindow
            {
                titleContent = new GUIContent("Localizer"),
                minSize = new Vector2(400, 85),
                maxSize = new Vector2(400, 85)
            };
            _instance.ShowUtility();
        }

        private string _keyValue = "";
        private string _languageValue = "";
        public void OnGUI()
        {
            EditorGUILayout.Space();

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
            _languageValue = EditorGUILayout.TextField(_languageValue, GUILayout.MaxWidth(175));
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Add") && !string.IsNullOrWhiteSpace(_languageValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.AddLanguage(_languageValue))
                    Debug.Log($"Localizer: successfully added language '{_languageValue}'.");
                else
                    Debug.LogError($"Localizer: failed to add language '{_languageValue}'.");
            }
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Remove") && !string.IsNullOrWhiteSpace(_languageValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.RemoveLanguage(_languageValue))
                    Debug.Log($"Localizer: successfully removed language '{_languageValue}'.");
                else
                    Debug.LogError($"Localizer: failed to remove language '{_languageValue}'.");
            }
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.Space();

            #region Add/Remove Key
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Key", GUILayout.MaxWidth(75));
            _keyValue = EditorGUILayout.TextField(_keyValue, GUILayout.MaxWidth(175));
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Add") && !string.IsNullOrWhiteSpace(_keyValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.AddKey(_keyValue))
                    Debug.Log($"Localizer: successfully added key '{_keyValue}'.");
                else
                    Debug.LogError($"Localizer: failed to add key '{_keyValue}'.");
            }
            if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(75)), "Remove") && !string.IsNullOrWhiteSpace(_keyValue))
            {
                CSVLoader csvLoader = new CSVLoader();
                if (csvLoader.RemoveKey(_keyValue))
                    Debug.Log($"Localizer: successfully removed key '{_keyValue}'.");
                else
                    Debug.LogError($"Localizer: failed to remove key '{_keyValue}'.");
            }
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        public void OnDestroy()
        {
            _instance = null;
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
                if (_config != null)
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
                _config = AssetDatabase.LoadAssetAtPath<LocalizationSystemConfig>(path);
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