using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.PhysicsColliders
{
    public class PolygonCollider : ColliderGeometry
    {
        new public float Rotation;
        public Vector2[] Vertices;
        public Vector2[] TransformedVertices;
        public PolygonCollider(Vector3 position, Vector2[] vertices) : base (position)
		{
            base.Shape = MathU.Geometry.Geometry.Shapes.Polygon;
            this.Vertices = vertices;
        }

        public void UpdatePolygon(Vector2 position, Vector3 rotation, Vector2 scale)
        {
            base.requiresUpdate = false;
            base.Position = position;
            this.Rotation = rotation.z;
        }

        public void UpdateVertices(Vector2[] vertices)
		{
            this.TransformedVertices = vertices;
        }
    }
}

