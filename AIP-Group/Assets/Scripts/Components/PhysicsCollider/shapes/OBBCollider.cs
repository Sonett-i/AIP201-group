using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;

namespace PhysicsEngine.PhysicsColliders
{
    // oriented bounding box
    public class OBBCollider : ColliderGeometry
    {
        public Vector2 Min;
        public Vector2 Max;

        float Width;
        float Height;

        new public float Rotation;
        public Vector2[] Vertices;
        public int[] Triangles;
        public Vector2[] TransformedVertices;
        public OBBCollider(Vector2 position, Vector2 scale, Vector3 rotation) : base (position, scale, rotation)
        {
            base.Shape = MathU.Geometry.Geometry.Shapes.OBB;

            this.Min = new Vector2(Position.x - scale.x / 2f, Position.y - scale.y / 2f);
            this.Max = new Vector2(Position.x + scale.x / 2f, Position.y + scale.y / 2f);

            this.Vertices = UpdateVertices();
            this.TransformedVertices = new Vector2[4];
            this.Triangles = CreateBoxTriangles();
        }

        private Vector2[] UpdateVertices()
		{
            Vector2[] vertices = new Vector2[4];
            vertices[0] = new Vector2(Min.x, Max.y);
            vertices[1] = new Vector2(Max.x, Max.y);
            vertices[2] = new Vector2(Max.x, Min.y);
			vertices[3] = new Vector2(Min.x, Min.y);

			Debug.DrawLine(vertices[0], this.Position, Color.green);
            Debug.DrawLine(vertices[1], this.Position, Color.green);
            Debug.DrawLine(vertices[2], this.Position, Color.green);
            Debug.DrawLine(vertices[3], this.Position, Color.green);

            return vertices;
		}

        private int[] CreateBoxTriangles()
		{
			int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            return triangles;
        }

        public Vector2[] GetTransformedVertices()
		{
            if (base.requiresUpdate)
			{
                MathU.Geometry.Transform transform = new MathU.Geometry.Transform(this.Position, this.Rotation);

                for (int i = 0; i < this.Vertices.Length; i++)
				{
                    Vector2 v = this.Vertices[i];
					this.TransformedVertices[i] = VectorUtils.Transform(v, transform);
				}
			}

            return this.TransformedVertices;
		}

        public void UpdateOBB(Vector2 position, Quaternion rotation, Vector2 scale)
        {
            this.Min = new Vector2(Position.x - scale.x / 2f, Position.y - scale.y / 2f);
            this.Max = new Vector2(Position.x + scale.x / 2f, Position.y + scale.y / 2f);

            this.Vertices = UpdateVertices();
            this.TransformedVertices = GetTransformedVertices();

            base.requiresUpdate = false;
        }
    }
}
