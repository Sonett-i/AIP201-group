using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    public class AABBCollider : PhysicsCollider
    {
        public Vector2 position;
        public Vector2 min;
        public Vector2 max;
    }
}

