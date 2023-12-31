using UnityEngine;

namespace Base
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = (T)FindObjectOfType(typeof(T));
            else if (Instance == this)
                return;
            else
                DestroyDuplicate();
        }

        private void DestroyDuplicate()
        {
            Destroy(this);
        }

        public static T Instance { get; protected set; }
    }
}