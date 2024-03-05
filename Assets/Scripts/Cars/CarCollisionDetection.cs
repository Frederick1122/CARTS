using ModestTree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cars.Tools
{
    public class CarCollisionDetection : MonoBehaviour, ICarCollision
    {
        public event Action OnNoCollidersIn;

        public bool CollisionCanTurnOn => _inColliders.IsEmpty();

        private bool _isWork = false;
        public bool IsWork
        {
            get { return _isWork; }
            set
            {
                if (value)
                {
                    //ClearColliders();
                    _isWork = true;
                    TurnCollisionOff();
                }
                else
                {
                    _isWork = false;
                    TurnCollisionOn();
                    ClearColliders();
                }
            }
        }

        private HashSet<Collider> _inColliders = new(); 
        private Collider _selfCollider = new();
        private LayerMask _layerMask;

        private HashSet<Collider> _worldColliders = new();
        private Dictionary<Collider, ICarCollision> _worldCollisions = new();

        public void Init(Collider selfCollider, LayerMask layerMask)
        {
            _selfCollider = selfCollider;
            _layerMask = layerMask;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isWork)
                return;

            if (!((_layerMask.value & (1 << other.gameObject.layer)) > 0))
                return;

            if (other.gameObject.TryGetComponent(out ICarCollision carCollision))
            {
                //Physics.IgnoreCollision(_selfCollider, other, true);
                //Physics.IgnoreCollision(other, _selfCollider, true);
                AddCollider(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isWork)
                return;

            if (!((_layerMask.value & (1 << other.gameObject.layer)) > 0))
                return;

            if (other.gameObject.TryGetComponent(out ICarCollision carCollision))
            {
                RemoveCollider(other);
            }
        }

        public void SetUpAllWorldCollider(List<Collider> worldColliders)
        {
            _worldColliders = worldColliders.ToHashSet();
            foreach (Collider c in worldColliders)
            {
                if(c.TryGetComponent(out ICarCollision collision))
                    _worldCollisions.Add(c, collision);
            }
        }

        public void TurnCollisionOn()
        {
            foreach (var colWorld in _worldColliders)
            {
                if (_selfCollider == colWorld)
                    continue;

                if (CheckCollision(colWorld))
                    continue;

                Physics.IgnoreCollision(_selfCollider, colWorld, false);
                Physics.IgnoreCollision(colWorld, _selfCollider, false);
            }
        }

        public void TurnCollisionOff()
        {
            foreach (var colWorld in _worldColliders)
            {
                if (_selfCollider == colWorld)
                    continue;

                Physics.IgnoreCollision(_selfCollider, colWorld, true);
            }
        }

        public void AddCollider(Collider col) =>
            _inColliders.Add(col);

        public void RemoveCollider(Collider col)
        {
            _inColliders.Remove(col);
            if (!_inColliders.IsEmpty())
                return;

            OnNoCollidersIn?.Invoke();
            TurnCollisionOn();
        }

        private void ClearColliders()
        {
            foreach (Collider col in _inColliders)
                RemoveCollider(col);
        }

        private bool CheckCollision(Collider col1)
        {
            var res = _worldCollisions.ContainsKey(col1);
            if (!res)
                return false;

            return _worldCollisions[col1].IsWork;
        }
    }
}

