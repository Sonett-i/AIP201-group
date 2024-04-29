using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;

namespace PhysicsEngine.PhysicsColliders
{
	public class ColliderGeometry
	{
		public Vector3 Position;
		public Vector3 Scale;
		public Vector3 Rotation;
		public Quaternion rotation;

		public Geometry.Shapes Shape;
		public bool requiresUpdate = true;

		public ColliderGeometry(Vector3 position)
		{
			this.Position = position;
			this.Scale = Vector3.zero;
			this.Rotation = Vector3.zero;
		}

		public ColliderGeometry(Vector3 position, Vector3 scale)
		{
			this.Position = position;
			this.Scale = scale;
			this.Rotation = Vector3.zero;
		}

		public ColliderGeometry(Vector3 position, params Vector3[] args)
		{
			this.Position = position;
			this.Scale = args[0];
			this.Rotation = args[1];
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
				OBB.UpdateOBB(position, rotation.eulerAngles, scale);
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
