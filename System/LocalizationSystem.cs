/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

namespace DonutStudio.LocalizationSystem
{
    /// <summary>
    /// Class to handle localization.
    /// </summary>
    public static class LocalizationSystem
    {
        public static LocalizationSystemConfig Config { get; private set; }
        public static bool IsInitialized { get; private set; } = false;
        public static int LanguageIndex { get; private set; } = 0;
        public static EventHandler onRefresh;

        private static bool _hasDictionary = false;
        private static Dictionary<string, string> _localizedLanguage;
        private static CSVLoader _csvLoader;


        #region System

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
            _csvLoader = new CSVLoader();

            // load the config file from the 'resources' folder
            Config = Resources.Load<LocalizationSystemConfig>("LocalizationSystemConfig");
            if (Config != null)
                IsInitialized = true;
            else
                Debug.LogError("Failed to load the localization system config file! Make sure it's in the 'Resources' folder.");
            
            return IsInitialized;
        }

        /// <summary>
        /// Swap the language to another one.
        /// </summary>
        /// <param name="languageIndex">The index of the language to change to.</param>
        /// <param name="refresh">Should the refresh event be called.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetLanguage(int languageIndex, bool refresh = true)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;

            // check if the same language is selected or if the index is out of range
            if ((LanguageIndex == languageIndex && _hasDictionary) || OutOfRange(languageIndex))
                return false;
            
            // set the language index
            LanguageIndex = languageIndex;

            // get all language attributes from the file and set the dictionary according to the selected language
            string[] attributes = _csvLoader.GetLanguageAttributes();
            _localizedLanguage = _csvLoader.GetDictionaryValues(attributes[languageIndex]);
            _hasDictionary = true;

            // invoke the onRefresh event
            if (refresh)
                RefreshTextElements();
            return true;
        }
        /// <summary>
        /// Swap the language to another one.
        /// </summary>
        /// <param name="language">The language to change to.</param>
        /// <param name="refresh">Should the refresh event be called.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetLanguage(Language language, bool refresh = true)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;

            // check if there are languages in the config file
            if (Config.languages == null || Config.languages.Length == 0)
                return false;

            // search for the index in the config file
            int i;
            for (i = 0; i < Config.languages.Length; i++)
                if (language.Equals(Config.languages[i]))
                    break;
            return SetLanguage(i, refresh);
        }
        
        /// <summary>
        /// Refresh all text elements that are listening to the event.
        /// </summary>
        public static void RefreshTextElements()
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return;

            if (_hasDictionary)
                onRefresh?.Invoke(null, EventArgs.Empty);
        }
        
        #endregion

        #region Get

        /// <summary>
        /// Get the currently selected language.
        /// </summary>
        /// <returns>The currently selected language.</returns>
        public static Language GetLanguage()
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return new Language();
            return Config.languages[LanguageIndex];
        }
        /// <summary>
        /// Get the currently active font.
        /// </summary>
        /// <returns>The currently active font, null on failure.</returns>
        public static TMP_FontAsset GetFont()
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return null;
            Language current = GetLanguage();
            return current.languageFont == null ? Config.standardFont : current.languageFont;
        }

        /// <summary>
        /// Try to get the localized string by a key.
        /// </summary>
        /// <param name="key">The key for the corresponding value.</param>
        /// <param name="value">The localized value.</param>
        /// <returns>True on sucess, false on failure.</returns>
        public static bool GetLocalizedValue(string key, out string value)
        {
            value = key;

            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            // check if the dictionary was populated
            if (!_hasDictionary)
                return false;

            return _localizedLanguage.TryGetValue(key, out value);
        }
        /// <summary>
        /// Try to get the localized string by a key.
        /// </summary>
        /// <param name="key">The key for the corresponding value.</param>
        /// <returns>The localized value on success or the key on failure.</returns>
        public static string GetLocalizedValue(string key)
        {
            // try to get the localized value
            if (GetLocalizedValue(key, out string value))
                return value;
            return key;
        }

        /// <summary>
        /// Try to phrase a given text with multiple keys. '{x}' will be replaced by the values; x beginning at 0.
        /// </summary>
        /// <param name="text">The text to phrase.</param>
        /// <param name="result">The result of the action.</param>
        /// <param name="keys">The keys to get the localized values.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool GetPhrasedText(string text, out string result, params string[] keys)
        {
            result = text;

            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            if (keys == null || keys.Length == 0)
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
        /// Try to phrase a given text with multiple keys. '{x}' will be replaced by the values; x beginning at 0.
        /// </summary>
        /// <param name="text">The text to phrase.</param>
        /// <param name="result">The result of the action.</param>
        /// <param name="localizedValues">The keys to get the localized values.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool GetPhrasedText(string text, out string result, params LocalizedValue[] localizedValues)
        {
            result = text;

            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            if (localizedValues == null || localizedValues.Length == 0)
                return false;

            string[] keys = new string[localizedValues.Length];
            for (int i = 0; i < localizedValues.Length; i++)
                keys[i] = localizedValues[i].key;
            return GetPhrasedText(text, out result, keys);
        }

        /// <summary>
        /// Try to phrase a given text with multiple keys. '{x}' will be replaced by the values; x beginning at 0.
        /// </summary>
        /// <param name="text">The text to phrase.</param>
        /// <param name="keys">The keys to get the localized values.</param>
        /// <returns>The phrased text on success, or the input text on failure.</returns>
        public static string GetPhrasedText(string text, params string[] keys)
        {
            if (GetPhrasedText(text, out string result, keys))
                return result;
            return text;
        }
        /// <summary>
        /// Try to phrase a given text with multiple keys. '{x}' will be replaced by the values; x beginning at 0.
        /// </summary>
        /// <param name="text">The text to phrase.</param>
        /// <param name="localizedValues">The keys to get the localized values.</param>
        /// <returns>The phrased text on success, or the input text on failure.</returns>
        public static string GetPhrasedText(string text, params LocalizedValue[] localizedValues)
        {
            if (GetPhrasedText(text, out string result, localizedValues))
                return result;
            return text;
        }

        #endregion;

        #region TextMeshPro

        /// <summary>
        /// Sets the text and font of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="key">The key for the value.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetTMP(TextMeshProUGUI uiElement, string key)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            SetFont(uiElement);
            return SetText(uiElement, key);
        }
        /// <summary>
        /// Sets the text and font of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="text">The text to phrase.</param>
        /// <param name="keys">The keys for the values to replace.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetTMP(TextMeshProUGUI uiElement, string text, params string[] keys)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
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
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
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
        /// <returns>True on success, false on failure.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, string key)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
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
        /// <param name="text">The text to phrase.</param>
        /// <param name="key">The keys for the values to replace.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, string text, params string[] keys)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            if (GetPhrasedText(text, out string result, keys))
            {
                uiElement.text = result;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets the text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="localizedValue">The localized value for the value.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, LocalizedValue localizedValue)
        {
            return SetText(uiElement, localizedValue.key);
        }
        /// <summary>
        /// Sets the text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <param name="text">The text to phrase.</param>
        /// <param name="localizedValues">The localized values for the values to replace.</param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SetText(TextMeshProUGUI uiElement, string text, params LocalizedValue[] localizedValues)
        {
            // try to initialize the system if it is not initialized, return on failure
            if (!CheckIsInitialized())
                return false;
            if (GetPhrasedText(text, out string result, localizedValues))
            {
                uiElement.text = result;
                return true;
            }
            return false;
        }

        #endregion

        #region Checks

        /// <summary>
        /// Check if the language index is outside of range.
        /// </summary>
        /// <param name="_languageIndex">The language index.</param>
        /// <returns>True if the language index is out of range, false if not</returns>
        private static bool OutOfRange(int languageIndex)
        {
            if (!IsInitialized)
                return true;
            return languageIndex < 0 || languageIndex >= Config.languages.Length;
        }
        /// <summary>
        /// Check if the system is initialized and tries to initialize it if not.
        /// </summary>
        /// <returns>True if the system is initialized, false if not.</returns>
        private static bool CheckIsInitialized()
        {
            // try to initialize the system if it is not initialized
            if (!IsInitialized && !Initialize())
                return false;
            return true;
        }

        #endregion
    }
}