using UnityEngine;

namespace Base
{
    public class Singleton<T> : MonoBehaviour, ICreate where T : MonoBehaviour 
    {
        protected virtual void Awake()
        {
            if (Instance == null) 
                return;
            
            Create();
        }

        public void Create()
        {
            if (Instance == null)
                Instance = GetComponent<T>();
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

    public interface ICreate
    {
        public void Create();
    }
}