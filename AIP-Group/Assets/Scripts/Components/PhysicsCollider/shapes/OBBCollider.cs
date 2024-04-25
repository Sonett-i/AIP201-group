using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    // oriented bounding box
    public class OBBCollider : ColliderGeometry
    {
        public Vector2 Min;
        public Vector2 Max;

        public Vector2[] Vertices = new Vector2[4];
        public OBBCollider(Vector2 position, Vector2 dimensions) : base (position)
        {
            base.Shape = MathU.Geometry.Geometry.Shapes.OBB;

            this.Min = new Vector2(Position.x - dimensions.x / 2f, Position.y - dimensions.y / 2f);
            this.Max = new Vector2(Position.x + dimensions.x / 2f, Position.y + dimensions.y / 2f);
        }

        public void UpdateVertices()
		{

		}

        public void UpdateOBB(Vector2 position, Quaternion rotation, Vector2 scale)
        {
            this.Min = new Vector2(Position.x - scale.x / 2f, Position.y - scale.y / 2f);
            this.Max = new Vector2(Position.x + scale.x / 2f, Position.y + scale.y / 2f);
        }
    }
}
