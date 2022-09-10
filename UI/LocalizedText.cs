/*
  Localization system - Extention for Unity use multiple languages in your game.
  Created by Donut Studio, September 10, 2022.
  Released into the public domain.
*/

using UnityEngine;
using TMPro;
using System;

namespace DonutStudio.Utilities.LocalizationSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField()] private LocalizedValue key;
        [SerializeField()] private bool refreshOnAwake = true;
        [SerializeField()] private bool refreshOnChange = true;

        private void Awake()
        {
            if (refreshOnAwake)
                RefreshText();
        }
        private void OnEnable()
        {
            if (refreshOnChange)
                LocalizationSystem.onRefresh += OnRefresh;
        }
        private void OnDisable()
        {
            if (refreshOnChange)
                LocalizationSystem.onRefresh -= OnRefresh;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            RefreshText();
        }
        public void RefreshText()
        {
            key.SetTMP(GetComponent<TextMeshProUGUI>());
        }
    }
}