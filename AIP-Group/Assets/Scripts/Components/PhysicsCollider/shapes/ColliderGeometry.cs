using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;

namespace PhysicsEngine.PhysicsColliders
{
	public class ColliderGeometry
	{
		public Vector3 Position;
		public Geometry.Shapes Shape;
		public bool requiresUpdate = true;

		public ColliderGeometry(Vector3 position)
		{
			this.Position = position;
		}

		public void Update(Vector2 position, Vector2 scale = default, Quaternion rotation = default)
		{
			this.Position = position;

			if (this is AABBCollider)
			{
				AABBCollider AABB = (AABBCollider)this;
				AABB.UpdateAABB(position, scale);
			}
			else if (this is OBBCollider)
			{
				OBBCollider OBB = (OBBCollider)this;
				OBB.UpdateOBB(position, rotation, scale);
			}
			else if (this is CircleCollider)
			{
				CircleCollider Circle = (CircleCollider)this;
				Circle.UpdateCircle(scale.x / 2f);
			}

			//update AABB

			requiresUpdate = false;
		}
	}
}
