/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    /// <summary>
    /// Class for reading from and writing to the csv file.
    /// </summary>
    public class CSVLoader
    {
        private readonly char lineSeperator = '\n';
        private readonly char fieldSeperator = ';';

        private TextAsset csvFile;
        
        /// <summary>
        /// Create a CSVLoader object and load the csv file from the resources folder.
        /// </summary>
        public CSVLoader()
        {
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
        /// <param name="attributeID">The language attribute to load from.</param>
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
                    // remove the carriage return to avoid overlapping
                    var key = fields[0].Replace("\r", "");
                    if (dictionary.ContainsKey(key)) { continue; }

                    var value = fields[attributeIndex].Replace("\r", "");
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
        public void AddKey(string key)
        {
            File.AppendAllText("Assets/Resources/localization.csv", lineSeperator + key + fieldSeperator);
            UnityEditor.AssetDatabase.Refresh();
        }
        /// <summary>
        /// Remove a key and its values from the csv file.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        public bool RemoveKey(string key)
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
        /// <summary>
        /// Add a new language to the csv file.
        /// </summary>
        /// <param name="language">The language to add.</param>
        public void AddLanguage(string language)
        {
            // split the file into the single lines
            string[] lines = csvFile.text.Split(lineSeperator);

            // add the language
            lines[0] += $";{language}";
            
            // write the result to the file
            File.WriteAllText("Assets/Resources/localization.csv", string.Join(lineSeperator.ToString(), lines));
            UnityEditor.AssetDatabase.Refresh();
        }
        /// <summary>
        /// Remove a language from the csv file.
        /// </summary>
        /// <param name="language">The language to remove.</param>
        public bool RemoveLanguage(string language)
        {
            // split the file into the single lines
            string[] lines = csvFile.text.Split(lineSeperator);

            // get the seperate values from the header
            List<string> headerV = lines[0].Split(fieldSeperator).ToList();
            // remove the language from the header values
            bool success = headerV.Remove(language);

            // join the lines together
            string header = string.Join(fieldSeperator.ToString(), headerV);
            List<string> linesN = lines.ToList();
            linesN.RemoveAt(0);
            
            File.WriteAllText("Assets/Resources/localization.csv", header + lineSeperator + string.Join(lineSeperator.ToString(), linesN));
            UnityEditor.AssetDatabase.Refresh();
            return success;
        }
        /// <summary>
        /// Sort the keys of the csv file.
        /// </summary>
        public void SortFile()
        {
            // split the file into lines and remove the header
            List<string> lines = csvFile.text.Split(lineSeperator).ToList();
            string header = lines[0];
            lines.RemoveAt(0);

            // sort the list
            lines.Sort((a, b) =>
            {
                string[] splits1 = a.Split('/');
                string[] splits2 = b.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                        return 1;

                    int value = splits1[i].CompareTo(splits2[i]);
                    if (value != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits2.Length - 1 || i == splits2.Length - 1))
                            return splits1.Length < splits2.Length ? 1 : -1;
                        return value;
                    }
                }

                return 0;
            });

            // write the result to the file
            File.WriteAllText("Assets/Resources/localization.csv", header + lineSeperator + string.Join(lineSeperator.ToString(), lines));
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}