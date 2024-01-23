using System.Collections.Generic;
using UnityEngine;

namespace Base.Pool
{
    public class PoolMono<T> where T : MonoBehaviour, IPoolObject
    {
        private static Transform _mainContainer;

        private readonly List<T> _prefabsList;
        private readonly bool _autoExpand;

        private Transform _objectsContainer;
        private List<T> _pool;

        private readonly Dictionary<T, int> _spawn;

        public PoolMono(T prefab, int count, bool autoExpand = true)
        {
            _prefabsList = new List<T>() { prefab };
            _autoExpand = autoExpand;

            _spawn = new();
            foreach (var item in _prefabsList)
                _spawn.Add(item, 0);

            CreateContainer();
            CreatePool(count);
        }

        public PoolMono(List<T> prefabList, int count, bool autoExpand = true)
        {
            _prefabsList = prefabList;
            _autoExpand = autoExpand;

            _spawn = new();
            foreach (var item in _prefabsList)
                _spawn.Add(item, 0);

            CreateContainer();
            CreatePool(count);
        }

        private void CreateContainer()
        {
            if (_mainContainer == null)
                _mainContainer = new GameObject("==PoolContainer==").transform;

            string containerName = $"{typeof(T).Name}_{_prefabsList[0].name}";
            _objectsContainer = new GameObject(containerName).transform;
            _objectsContainer.SetParent(_mainContainer);
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
                CreateObject();
        }

        private T CreateObject(bool activeByDefault = false)
        {
            T prefab = (_prefabsList.Count == 1) ? _prefabsList[0] : GetRandomPrefab();
            T createdObject = Object.Instantiate(prefab, _objectsContainer);
            createdObject.gameObject.SetActive(activeByDefault);
            createdObject.OnObjectNeededToDeactivate += WhenObjectDeactivate;

            if (!activeByDefault)
                _pool.Add(createdObject);
            return createdObject;
        }

        private T GetRandomPrefab()
        {
            T obj = _prefabsList[0];
            int min = _spawn[obj];

            foreach (var item in _spawn)
            {
                if (item.Value < min)
                {
                    obj = item.Key;
                    min = item.Value;
                }
            }

            _spawn[obj]++;

            return obj;
        }

        public T GetObject()
        {
            if (HasFreeElement(out var element))
            {
                _pool.Remove(element);
                return element;
            }

            if (_autoExpand)
                return CreateObject(true);

            throw new System.Exception($"There is no free element in pool of type {typeof(T)} or pool is not auto expand");
        }

        private bool HasFreeElement(out T element)
        {
            if (_pool.Count != 0)
            {
                element = _pool[Random.Range(0, _pool.Count)];
                element.gameObject.SetActive(true);
                return true;
            }
            element = null;
            return false;
        }

        private void WhenObjectDeactivate(IPoolObject obj)
        {
            obj.ResetBeforeBackToPool();
            (obj as T).transform.SetParent(_objectsContainer);
            _pool.Add(obj as T);
        }
    }
}