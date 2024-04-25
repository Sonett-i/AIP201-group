using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;

namespace PhysicsEngine.Colliders
{
	public class ColliderGeometry
	{
		public Vector3 position;
		public Geometry.Shapes shape;

		public ColliderGeometry(Vector3 position)
		{
			this.position = position;
		}
	}
}
