using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    // Axis-Aligned bounding box
    public class AABBCollider : ColliderGeometry
    {
        public Vector2 Min;
        public Vector2 Max;

        public AABBCollider(Vector2 position, Vector2 dimensions) : base (position)
		{
            base.Shape = MathU.Geometry.Geometry.Shapes.AABB;

            this.Min = new Vector2(Position.x - dimensions.x / 2f, Position.y - dimensions.y / 2f);
            this.Max = new Vector2(Position.x + dimensions.x / 2f, Position.y + dimensions.y / 2f);
		}

        public void UpdateAABB(Vector2 position, Vector2 dimensions)
		{
            this.Min = new Vector2(Position.x - dimensions.x / 2f, Position.y - dimensions.y / 2f);
            this.Max = new Vector2(Position.x + dimensions.x / 2f, Position.y + dimensions.y / 2f);
        }
    }
}

