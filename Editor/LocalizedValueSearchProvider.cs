/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DonutStudio.LocalizationSystem
{
    public class LocalizedValueSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private string[] _keys;
        private Action<string> _onSetIndexCallback;

        public LocalizedValueSearchProvider(string[] keys, Action<string> callback)
        {
            _keys = keys;
            _onSetIndexCallback = callback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("List"), 0));
            
            // create the search tree for the keys
            List<string> groups = new List<string>();
            foreach(string item in _keys)
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
            _onSetIndexCallback?.Invoke((string)SearchTreeEntry.userData);
            return true;
        }
    }
}