using UnityEngine;

namespace Core.Data
{
    public static class PlayerPrefsSaveLoadManager
    {
        #region Save
        public static void SaveInt(string key, int value) =>
            PlayerPrefs.SetInt(key, value);

        public static void SaveFloat(string key, float value) =>
            PlayerPrefs.SetFloat(key, value);

        public static void SaveString(string key, string value) =>
            PlayerPrefs.SetString(key, value);
        #endregion

        #region Load
        public static int LoadInt(string key)
        {
            if (!HasKey(key))
                return int.MaxValue;
            return PlayerPrefs.GetInt(key);
        }

        public static float LoadFloat(string key)
        {
            if (!HasKey(key))
                return float.MaxValue;
            return PlayerPrefs.GetFloat(key);
        }

        public static string LoadString(string key)
        {
            if (!HasKey(key))
                return string.Empty;
            return PlayerPrefs.GetString(key);
        }
        #endregion

        #region Delete
        private static void DeleteKey(string key) =>
            PlayerPrefs.DeleteKey(key);

        private static void DeleteAll() =>
            PlayerPrefs.DeleteAll();
        #endregion

        public static bool HasKey(string key)
        {
           return PlayerPrefs.HasKey(key);
        }
    }
}
