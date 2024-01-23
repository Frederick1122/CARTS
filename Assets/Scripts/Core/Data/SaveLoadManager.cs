using UnityEngine;

namespace Base
{
    public abstract class SaveLoadManager<T, T2> : Singleton<T2> where T2 : MonoBehaviour
    {
        protected string _secondPath = "";
        private string _fullPath = "";

        protected T _saveData;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        protected virtual void OnApplicationQuit() => Save();

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                Save();
        }

        protected virtual void Load()
        {
            UpdatePath();
            _saveData = DataSaver.LoadData<T>(_secondPath);
        }

        protected void Save()
        {
            UpdatePath();
            DataSaver.SaveData(_saveData, _secondPath);
        }

        protected virtual void UpdatePath()
        {
            if (_fullPath == "")
                _fullPath = Application.streamingAssetsPath + _secondPath;
        }
    }
}