/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
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
            // get the property from the 'LocalizedValue' class
            SerializedProperty keyProperty = property.FindPropertyRelative("key");

            // draw the label field and button
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(position, property.displayName);
            if (GUI.Button(new Rect(position.x * 8, position.y, position.width / 2, position.height), keyProperty.stringValue.Split('/').Last(), EditorStyles.popup))
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), new LocalizedValueSearchProvider(new CSVLoader().GetKeys(), (x) => { keyProperty.stringValue = x; keyProperty.serializedObject.ApplyModifiedProperties(); } ));
            EditorGUI.EndProperty();
        }
    }
}