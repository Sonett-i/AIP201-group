using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.PhysicsColliders
{
    public class CircleCollider : ColliderGeometry
	{
        public float Radius;

        public CircleCollider(Vector3 position, float radius) : base (position)
		{
			base.Shape = MathU.Geometry.Geometry.Shapes.Circle;

			this.Radius = radius;
		}

		public void UpdateCircle(float radius)
		{
			Radius = radius;
		}
	}
}

