/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using System;
using UnityEngine;
using TMPro;

namespace DonutStudio.LocalizationSystem
{
    public class LocalizationSystemConfig : ScriptableObject
    {
        [Tooltip("The standard font every language uses.")] public TMP_FontAsset standardFont;
        public Language[] languages = new Language[] { new Language("English") };
    }

    [Serializable]
    public struct Language
    {
        [Tooltip("The name of the language.")] 
        public string languageName;
        [Tooltip("The specific font for the language (leave empty to use the standard font).")] 
        public TMP_FontAsset languageFont;

        /// <summary>
        /// Create a language with the given name.
        /// </summary>
        /// <param name="_name">The name of the language.</param>
        public Language(string _name)
        {
            languageName = _name;
            languageFont = null;
        }
        /// <summary>
        /// Compare this language with another one.
        /// </summary>
        /// <param name="value">The other language.</param>
        /// <returns>True when the languages are identical and false if not.</returns>
        public bool Equals(Language value)
        {
            return languageName.Equals(value.languageName);
        }
    }
}