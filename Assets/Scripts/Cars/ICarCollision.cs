using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cars.Tools
{
    public interface ICarCollision
    {
        public bool IsWork { get; }

        public void AddCollider(Collider col);
        public void RemoveCollider(Collider col);
    }
}
