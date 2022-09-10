/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using TMPro;

namespace DonutStudio.Utilities.LocalizationSystem
{
    [System.Serializable()]
    public class LocalizedValue
    {
        public string key;

        /// <summary>
        /// Returns the value of the key.
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            LocalizationSystem.GetLocalizedValue(key, out string value);
            return value;
        }
        /// <summary>
        /// Returns true if a value for the key was found and false if not.
        /// </summary>
        /// <param name="value">The value of the key.</param>
        /// <returns></returns>
        public bool GetValue(out string value)
        {
            return LocalizationSystem.GetLocalizedValue(key, out value);
        }

        /// <summary>
        /// Sets the font and text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public bool SetTMP(TextMeshProUGUI uiElement)
        {
            return LocalizationSystem.SetTMP(uiElement, key);
        }
        /// <summary>
        /// Sets the text of a tmp text element.
        /// </summary>
        /// <param name="uiElement">The tmp text element.</param>
        /// <returns>Returns true on sucess and false when failed.</returns>
        public bool SetText(TextMeshProUGUI uiElement)
        {
            return LocalizationSystem.SetText(uiElement, key);
        }
    }
}