/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, June 26, 2022.
  Released into the public domain.
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public class CSVLoader
    {
        private readonly char lineSeperator = '\n';
        private readonly char fieldSeperator = ';';

        private TextAsset csvFile;
        
        public CSVLoader()
        {
            // load the csv-file
            csvFile = Resources.Load<TextAsset>("localization");
        }
        /// <summary>
        /// Returns the language attributes from the csv file.
        /// </summary>
        /// <returns></returns>
        public string[] GetLanguageAttributes()
        {
            // split the file into the header and single fields (attributes)
            string[] lines = csvFile.text.Split(lineSeperator);
            string[] headers = lines[0].Split(fieldSeperator);

            // define a new array with the attributes
            string[] attributes = new string[headers.Length - 1];

            // add them into the array
            for (int i = 1; i < headers.Length; i++)
                attributes[i - 1] = headers[i];

            return attributes;
        }
        /// <summary>
        /// Returns the keys from the csv file.
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            // split the file into single lines and define the array
            string[] lines = csvFile.text.Split(lineSeperator);
            string[] attributes = new string[lines.Length - 1];

            // add the keys into the array
            for (int i = 1; i < lines.Length; i++)
                attributes[i - 1] = lines[i].Split(fieldSeperator)[0];

            return attributes;
        }
        /// <summary>
        /// Retruns a dictionary with the keys and localized values from one language.
        /// </summary>
        /// <param name="attributeID">the language attribute</param>
        /// <returns></returns>
        public Dictionary<string, string> GetDictionaryValues(string languageAttribute)
        {
            // define the dictionary to return
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // split the file into the header and single fields (attributes)
            string[] lines = csvFile.text.Split(lineSeperator);
            string[] headers = lines[0].Split(fieldSeperator);

            // find the language attribute it in the header
            int attributeIndex = -1;
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(languageAttribute))
                {
                    attributeIndex = i;
                    break;
                }
            }

            // go through all lines (except the header), find the field with the proper attribute and add them to the dictionary
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(fieldSeperator);

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];
                    if (dictionary.ContainsKey(key)) { continue; }

                    var value = fields[attributeIndex];
                    dictionary.Add(key, value);
                }
            }
            return dictionary;
        }


#if UNITY_EDITOR
        /// <summary>
        /// Add a new key to the csv file.
        /// </summary>
        /// <param name="key">The key to add.</param>
        public void Add(string key)
        {
            File.AppendAllText("Assets/Resources/localization.csv", lineSeperator + key + fieldSeperator);
            UnityEditor.AssetDatabase.Refresh();
        }
        /// <summary>
        /// Remove a key and its values from the csv file.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public bool Remove(string key)
        {
            // split the file into the single lines
            string[] lines = csvFile.text.Split(lineSeperator);

            // go through all the keys and find the corresponding to remove
            for (int i = 1; i < lines.Length; i++)
            {
                string currentKey = lines[i].Split(fieldSeperator)[0];

                if(key.Equals(currentKey))
                {
                    string[] newLines = lines.Where(w => w != lines[i]).ToArray();

                    string replaced = string.Join(lineSeperator.ToString(), newLines);
                    File.WriteAllText("Assets/Resources/localization.csv", replaced);

                    UnityEditor.AssetDatabase.Refresh();
                    return true;
                }
            }
            
            return false;
        }
#endif
    }
}