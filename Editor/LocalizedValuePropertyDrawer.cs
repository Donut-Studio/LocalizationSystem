/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, May 29, 2022.
  Released into the public domain.
*/

using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    [CustomPropertyDrawer(typeof(LocalizedValue))]
    public class LocalizedValuePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty keyProperty = property.FindPropertyRelative("key");
            EditorGUI.BeginProperty(position, label, property);

            if(GUI.Button(position, keyProperty.stringValue.Split('/').Last(), EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), new LocalizedValueSearchProvider(new CSVLoader().GetKeys(), (x) => { keyProperty.stringValue = x; keyProperty.serializedObject.ApplyModifiedProperties(); } ));
            }
            
            EditorGUI.EndProperty();
        }
    }
}