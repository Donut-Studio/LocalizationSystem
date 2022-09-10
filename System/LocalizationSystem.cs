/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public static class LocalizationSystem
    {
        public static LocalizationSystemConfig Config { get; private set; }
        public static bool IsInitialized { get; private set; } = false;
        public static int LanguageIndex { get; private set; } = 0;
        public static EventHandler onRefresh;

        private static bool hasDictionary = false;
        private static Dictionary<string, string> localizedLanguage;
        private static CSVLoader csvLoader;

        /// <summary>
        /// Initialize the localization system. 
        /// </summary>
        /// <returns>True on success and false if an error occured.</returns>
        public static bool Initialize()
        {
            // return if the localization system is already initialized
            if (IsInitialized)
                return false;

            // create an object of the CSVLoader class
            csvLoader = new CSVLoader();

            // load the config file from the 'resources' folder
            Config = Resources.Load<LocalizationSystemConfig>("LocalizationSystemConfig");
            if (Config != null)
                IsInitialized = true;
            else
                Debug.LogError("Failed to load the localization system config file! Make sure it's in the 'Resources' folder.");
            
            return IsInitialized;
        }
        /// <summary>
        /// Swap the language to a new one and update the dictionary.
        /// </summary>
        /// <param name="_language">the language</param>
        /// <param name="refresh">refresh the text afterwards</param>
        public static bool SetLanguage(int _languageIndex, bool refresh = true)
        {
            // return if not initialized, the same language selected or out of range
            if (!IsInitialized || (LanguageIndex == _languageIndex && hasDictionary) || OutOfRange(_languageIndex))
                return false;
            
            // set the language
            LanguageIndex = _languageIndex;

            // get all language attributes from the file and set the dictionary according to the selected language
            string[] attributes = csvLoader.GetLanguageAttributes();
            localizedLanguage = csvLoader.GetDictionaryValues(attributes[_languageIndex]);

            hasDictionary = true;

            // invoke the onRefresh event
            if (refresh)
                RefreshTextElements();
            return true;
        }
        /// <summary>
        /// Refresh the text elements that are listening to the event.
        /// </summary>
        public static void RefreshTextElements()
        {
            if (IsInitialized && hasDictionary)
                onRefresh?.Invoke(null, EventArgs.Empty);
        }
        /// <summary>
        /// Try to get the localized string with a key.
        /// </summary>
        /// <param name="key">The key for the corresponding value.</param>
        /// <param name="value">The localized value.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public static bool GetLocalizedValue(string key, out string value)
        {
            value = key;
            if (!IsInitialized || !hasDictionary)
                return false;
            return localizedLanguage.TryGetValue(key, out value);
        }
        
        /// <summary>
        /// Returns true if the language index is outside of the range and false if not.
        /// </summary>
        /// <param name="_languageIndex">The language index.</param>
        /// <returns></returns>
        public static bool OutOfRange(int _languageIndex)
        {
            if (!IsInitialized)
                return true;
            return _languageIndex < 0 || _languageIndex >= Config.languages.Length;
        }
        /// <summary>
        /// Get the currently selected language.
        /// </summary>
        /// <returns>Returns the currently selected language.</returns>
        public static Language GetLanguage()
        {
            if (!IsInitialized)
                return new Language();
            return Config.languages[LanguageIndex];
        }
        /// <summary>
        /// Get the font used for the currently selected language.
        /// </summary>
        /// <returns>The font for the language</returns>
        public static TMP_FontAsset GetFont()
        {
            if (!IsInitialized)
                return null;
            Language current = GetLanguage();
            return current.languageFont == null ? Config.standardFont : current.languageFont;
        }

        /// <summary>
        /// Try to phrase a given text with multiple keys.
        /// </summary>
        /// <param name="text">The text to phrase.</param>
        /// <param name="result">The result of the action.</param>
        /// <param name="keys">The keys for the values to replace.</param>
        /// <returns></returns>
        public static bool GetPhrasedText(string text, out string result, params string[] keys)
        {
            result = text;
            if (!IsInitialized)
                return false;

            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                if (GetLocalizedValue(key, out string value))
                    result = result.Replace("{" + i + "}", value);
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Sets the text and font of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="key">The key for the value.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public static bool SetTMP(TextMeshProUGUI uiElement, string key)
        {
            if (!IsInitialized)
                return false;
            SetFont(uiElement);
            return SetText(uiElement, key);
        }
        /// <summary>
        /// Sets the text and font of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="text">The text to replace '{i}' with the value of the keys.</param>
        /// <param name="keys">The keys for the values to replace.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public static bool SetTMP(TextMeshProUGUI uiElement, string text, params string[] keys)
        {
            if (!IsInitialized)
                return false;
            SetFont(uiElement);
            return SetText(uiElement, text, keys);
        }
        /// <summary>
        /// Sets the font a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        public static void SetFont(TextMeshProUGUI uiElement)
        {
            if (!IsInitialized)
                return;
            TMP_FontAsset font = GetFont();
            if (font != null)
                uiElement.font = font;
        }
        /// <summary>
        /// Sets the text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="key">The key for the value.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, string key)
        {
            if (!IsInitialized)
                return false;
            if (GetLocalizedValue(key, out string value))
            {
                uiElement.text = value;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Sets the text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="text">The text to replace '{i}' with the value of the keys.</param>
        /// <param name="key">The keys for the values to replace.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, string text, params string[] keys)
        {
            if (!IsInitialized)
                return false;
            if (GetPhrasedText(text, out string result, keys))
            {
                uiElement.text = result;
                return true;
            }
            return false;
        }
    }
}