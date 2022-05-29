/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, May 29, 2022.
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
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("List"), 0)
            };

            List<string> groups = new List<string>();
            foreach(string item in keys)
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