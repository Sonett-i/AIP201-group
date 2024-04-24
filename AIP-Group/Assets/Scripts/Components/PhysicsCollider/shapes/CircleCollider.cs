using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    public class CircleCollider : ColliderGeometry
	{
        public Vector2 position;
        public float radius;

        public CircleCollider(Vector2 position, float radius)
		{
			this.position = position;
			this.radius = radius;
		}
	}
}

