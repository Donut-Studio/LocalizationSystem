/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, June 26, 2022.
  Released into the public domain.
*/

namespace DonutStudio.Utilities.LocalizationSystem
{
    [System.Serializable()]
    public class LocalizedValue
    {
        public string key;

        public bool GetValue(out string value)
        {
            return LocalizationSystem.GetLocalizedValue(key, out value);
        }
    }
}