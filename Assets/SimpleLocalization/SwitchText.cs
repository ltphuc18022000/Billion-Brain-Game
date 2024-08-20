using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
    /// <summary>
    /// Asset usage example.
    /// </summary>
    public class SwitchText : MonoBehaviour
    {
        private bool isEnglish = true; // Add a boolean to store current language state

        /// <summary>
        /// Called on app start.
        /// </summary>
        public void Awake()
        {
            LocalizationManager.Read();

            // Set initial language based on system language
            switch (Application.systemLanguage)
            {
                case SystemLanguage.English:
                    LocalizationManager.Language = "English";
                    isEnglish = true; // Set isEnglish to true if system language is English
                    break;

                default:
                    LocalizationManager.Language = "Vietnam";
                    isEnglish = false; // Set isEnglish to false if system language is not English
                    break;
            }
        }

        /// <summary>
        /// Change localization at runtime
        /// </summary>
        public void ToggleLocalization()
        {
            if (isEnglish)
            {
                LocalizationManager.Language = "Vietnam"; // If current language is English, switch to Vietnamese
                isEnglish = false;
            }
            else
            {
                LocalizationManager.Language = "English"; // If current language is Vietnamese, switch to English
                isEnglish = true;
            }
            PlayerPrefs.SetString("Language", LocalizationManager.Language);
        }
        private void Start()
        {
            // Read the language setting from PlayerPrefs
            if (PlayerPrefs.HasKey("Language"))
            {
                LocalizationManager.Language = PlayerPrefs.GetString("Language");
                isEnglish = LocalizationManager.Language == "English";
            }
        }
        /// <summary>
        /// Write a review.
        /// </summary>
    }
}
