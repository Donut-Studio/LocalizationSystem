/*
  Localization system - Extention for Unity to use multiple languages in your game.
  Created by Donut Studio, August 09, 2023.
  Released into the public domain.
*/

using UnityEngine;
using TMPro;
using System;

namespace DonutStudio.LocalizationSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField()] private LocalizedValue _key;
        [SerializeField()] private bool _refreshOnAwake = true;
        [SerializeField()] private bool _refreshOnChange = true;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            if (_refreshOnAwake)
                RefreshText();
        }
        private void OnEnable()
        {
            if (_refreshOnChange)
                LocalizationSystem.onRefresh += OnRefresh;
        }
        private void OnDisable()
        {
            if (_refreshOnChange)
                LocalizationSystem.onRefresh -= OnRefresh;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            RefreshText();
        }

        /// <summary>
        /// Fetch the value for the key and update the tmp element.
        /// </summary>
        public void RefreshText()
        {
            if (_text == null)
                _text = GetComponent<TextMeshProUGUI>();
            _key.SetTMP(_text);
        }
    }
}