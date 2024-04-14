using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cars.Tools
{
    public interface IWorldCollisionsSetUpper
    {
        public Dictionary<CarCollisionDetection, Collider> AllCollisions { get;}

        public void AddCollision(CarCollisionDetection collision, Collider collider);

        public void InitCollisionsDetetections();
    }
}
