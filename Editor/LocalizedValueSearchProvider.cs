/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, June 26, 2022.
  Released into the public domain.
*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DonutStudio.Utilities.LocalizationSystem
{
    public class LocalizedValueSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private string[] keys;
        private Action<string> onSetIndexCallback;

        public LocalizedValueSearchProvider(string[] _keys, Action<string> callback)
        {
            keys = _keys;
            onSetIndexCallback = callback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("List"), 0));

            // sort the keys
            List<string> sortedListItems = keys.ToList();
            sortedListItems.Sort((a, b) =>
            {
                string[] splits1 = a.Split('/');
                string[] splits2 = b.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= splits2.Length)
                        return 1;

                    int value = splits1[i].CompareTo(splits2[i]);
                    if(value != 0)
                    {
                        if (splits1.Length != splits2.Length && (i == splits2.Length - 1 || i == splits2.Length - 1))
                            return splits1.Length < splits2.Length ? 1 : -1;
                        return value;
                    }
                }

                return 0;
            });

            // create the search tree for the keys
            List<string> groups = new List<string>();
            foreach(string item in sortedListItems)
            {
                string[] entryTitle = item.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName = entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        searchList.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += '/';
                }
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()))
                {
                    level = entryTitle.Length,
                    userData = item
                };
                searchList.Add(entry);
            }

            return searchList;
        }
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            onSetIndexCallback?.Invoke((string)SearchTreeEntry.userData);
            return true;
        }
    }
}