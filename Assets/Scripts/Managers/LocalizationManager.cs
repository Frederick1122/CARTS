using System;
using System.Collections.Generic;
using Base;
using Knot.Localization;
using Knot.Localization.Data;

namespace Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        private List<KnotLanguageData> _languages = new List<KnotLanguageData>();
        
        protected override void Awake()
        {
            base.Awake();

            _languages.AddRange(KnotLocalization.Manager.Languages);
            SetLocalization(LocalizationLanguage.Russian);
        }

        public void SetLocalization(LocalizationLanguage language)
        {
            KnotLocalization.Manager.LoadLanguage(_languages[(int)language]);
        }
    }

    public enum LocalizationLanguage
    {
        English = 0,
        Russian = 1
    }
}