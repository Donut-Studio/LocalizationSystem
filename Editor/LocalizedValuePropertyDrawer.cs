/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, March 05, 2022.
  Released into the public domain.
*/

using UnityEditor;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    [CustomPropertyDrawer(typeof(LocalizedValue))]
    public class LocalizedValuePropertyDrawer : PropertyDrawer
    {
        bool gotKeys;
        int index;
        string[] keys;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!gotKeys)
            {
                CSVLoader csvLoader = new CSVLoader();
                keys = csvLoader.GetKeys();
                gotKeys = true;
            }
            
            SerializedProperty keyProperty = property.FindPropertyRelative("key");

            index = GetIndex(keyProperty.stringValue);
            index = EditorGUI.Popup(position, index, keys);

            keyProperty.stringValue = keys[index];
        }
        
        private int GetIndex(string key)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == key)
                    return i;
            }
            return 0;
        }
    }
}