/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;

namespace DonutStudio.LocalizationSystem
{
    [CustomPropertyDrawer(typeof(LocalizedValue))]
    public class LocalizedValuePropertyDrawer : PropertyDrawer
    {
        private const float LABELMULTIPLIER = 0.3f;
        private const float TEXTFIELDMULTIPLIER = 0.5f;
        private const float BUTTONMULTIPLIER = 0.2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // get the property from the 'LocalizedValue' class
            SerializedProperty keyProperty = property.FindPropertyRelative("key");

            // save the values of the rect
            float x = position.x;
            float y = position.y;
            float width = position.width;
            float height = position.height;


            // --- begin the property ---
            EditorGUI.BeginProperty(position, label, property);
    
            // create a rect
            Rect r = new Rect(x, y, width * LABELMULTIPLIER, height);

            // show an label with the display name of the property
            EditorGUI.LabelField(r, property.displayName);

            // change the rect for the next field
            r = MoveByWidth(r, width, LABELMULTIPLIER);
            r.width = width * TEXTFIELDMULTIPLIER;

            // show an text field for the user to enter the key
            string text = EditorGUI.TextField(r, keyProperty.stringValue);
            if (text != keyProperty.stringValue)
            {
                keyProperty.stringValue = text;
                keyProperty.serializedObject.ApplyModifiedProperties();
            }

            // change the rect for the next field
            r = MoveByWidth(r, width, TEXTFIELDMULTIPLIER);
            r.width = width * BUTTONMULTIPLIER;

            // show an button for the user to select the key from a dropdown
            if (GUI.Button(r, "Select", EditorStyles.miniButton))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), 
                    new LocalizedValueSearchProvider(new CSVLoader().GetKeys(), 
                    (selection) =>
                    {
                        keyProperty.stringValue = selection;
                        keyProperty.serializedObject.ApplyModifiedProperties();
                    }));
            }

            // --- end the property ---
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Moves the x position by the width multiplied with the multiplier.
        /// </summary>
        /// <param name="rect">The rect to manipulate.</param>
        /// <param name="width">The width.</param>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The manipulated rect.</returns>
        Rect MoveByWidth(Rect rect, float width, float multiplier)
        {
            rect.x += width * multiplier;
            return rect;
        }
    }
}