/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using TMPro;

namespace DonutStudio.LocalizationSystem
{
    /// <summary>
    /// Class for easier usage of the localization system in the editor
    /// </summary>
    [System.Serializable()]
    public class LocalizedValue
    {
        public string key;

        /// <summary>
        /// Get the value of the key.
        /// </summary>
        /// <returns>The value of the key on success, or the key on failure.</returns>
        public string GetValue()
        {
            return LocalizationSystem.GetLocalizedValue(key);
        }
        /// <summary>
        /// Try to get the localized string by a key.
        /// </summary>
        /// <param name="value">The localized value.</param>
        /// <returns>True on sucess, false on failure.</returns>
        public bool GetValue(out string value)
        {
            return LocalizationSystem.GetLocalizedValue(key, out value);
        }

        /// <summary>
        /// Sets the font and text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <returns>True on sucess, false on failure.</returns>
        public bool SetTMP(TextMeshProUGUI uiElement)
        {
            return LocalizationSystem.SetTMP(uiElement, key);
        }
    }
}