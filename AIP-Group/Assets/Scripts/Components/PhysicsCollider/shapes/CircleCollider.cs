using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    public class CircleCollider : ColliderGeometry
	{
        public float radius;

        public CircleCollider(Vector3 position, float radius) : base (position)
		{
			this.radius = radius;
		}
	}
}

