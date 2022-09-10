/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using System;
using UnityEngine;
using TMPro;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public class LocalizationSystemConfig : ScriptableObject
    {
        [Tooltip("The standard font every language uses.")] public TMP_FontAsset standardFont;
        public Language[] languages = new Language[] { Language.GetEnglish() };
    }

    [Serializable]
    public struct Language
    {
        [Tooltip("The name of the language.")] public string languageName;
        [Tooltip("A specific font to use for the language (leave empty to use the standard font).")] public TMP_FontAsset languageFont;

        /// <summary>
        /// Create a language with the given name.
        /// </summary>
        /// <param name="_name">The name of the langauge.</param>
        public Language(string _name)
        {
            languageName = _name;
            languageFont = null;
        }
        /// <summary>
        /// Compare this language with another one.
        /// </summary>
        /// <param name="value">The other language.</param>
        /// <returns>Returns true when identical and false if not.</returns>
        public bool CompareTo(Language value)
        {
            return languageName.Equals(value.languageName, StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Get the language for English.
        /// </summary>
        /// <returns>Returns a struct representing the English language</returns>
        public static Language GetEnglish()
        {
            return new Language("English");
        }
    }
}