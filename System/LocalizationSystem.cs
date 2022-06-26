/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, June 26, 2022.
  Released into the public domain.
*/

using System.Collections.Generic;
using System;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public static class LocalizationSystem
    {
        private static bool hasDictionary = false;
        private static Dictionary<string, string> localizedLanguage;

        public static Language Language { get; private set; } = Language.English;
        public static EventHandler onRefresh;

        /// <summary>
        /// Swap the language to a new one and update the dictionary.
        /// </summary>
        /// <param name="_language">the language</param>
        /// <param name="refresh">refresh the text afterwards</param>
        public static void SetLanguage(Language _language, bool refresh = true)
        {
            if (Language == _language && hasDictionary)
                return;

            // create a new CSVLoader object
            CSVLoader csvLoader = new CSVLoader();

            // set the language
            Language = _language;

            // get all language attributes from the file and set the dictionary according to the selected language
            string[] attributes = csvLoader.GetLanguageAttributes();
            localizedLanguage = csvLoader.GetDictionaryValues(attributes[(int)Language]);

            hasDictionary = true;

            // refresh the text
            if (refresh)
                RefreshTextElements();
        }
        /// <summary>
        /// Refresh the text elements that are listening to the event.
        /// </summary>
        public static void RefreshTextElements()
        {
            if (hasDictionary)
                onRefresh?.Invoke(null, EventArgs.Empty);
        }
        /// <summary>
        /// Try to get the localized string with a key.
        /// </summary>
        /// <param name="key">The key for the corresponding value.</param>
        /// <param name="value">The localized value.</param>
        /// <returns>Returns true on sucess, false if nothing was found.</returns>
        public static bool GetLocalizedValue(string key, out string value)
        {
            value = key;
            if (!hasDictionary)
                return false;
            return localizedLanguage.TryGetValue(key, out value);
        }
    }
}