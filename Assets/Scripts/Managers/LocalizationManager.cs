using System;
using Base;
using Knot.Localization;

namespace Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private const string RUSSIAN = "Russian";
        private const string ENGLISH = "English";

        public void SetLocalization(LocalizationLanguage language)
        {
            switch (language)
            {
                case LocalizationLanguage.Russian:
                    //LeanLocalization.SetCurrentLanguageAll(RUSSIAN);
                    break;
                case LocalizationLanguage.English:
                    //LeanLocalization.SetCurrentLanguageAll(ENGLISH);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }

    public enum LocalizationLanguage
    {
        Russian,
        English
    }
}