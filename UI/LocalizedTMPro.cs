/*
  Localization system - Extention for Unity to enable localization in your game.
  Created by Donut Studio, March 05, 2022.
  Released into the public domain.
*/

using UnityEngine;
using TMPro;
using System;

namespace DonutStudio.Utilities.LocalizationSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTMPro : MonoBehaviour
    {
        [SerializeField()]
        private LocalizedValue key;
        [SerializeField()]
        private bool refreshOnChange = true;

        private void Awake()
        {
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
        private void RefreshText()
        {
            if (LocalizationSystem.GetLocalizedValue(key.key, out string value))
                GetComponent<TextMeshProUGUI>().text = value;
        }
    }
}