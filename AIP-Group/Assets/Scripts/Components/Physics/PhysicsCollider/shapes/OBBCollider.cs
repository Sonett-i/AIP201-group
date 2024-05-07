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
        public Vector2[] Vertices = new Vector2[4];
        public int[] Triangles = new int[6];
        public Vector2[] TransformedVertices = new Vector2[4];

        public OBBCollider(Vector2 position, Vector2 scale, Vector3 rotation) : base (position, scale, rotation)
        {
            base.Shape = MathU.Geometry.Geometry.Shapes.OBB;
            this.Triangles = CreateBoxTriangles();
            this.UpdateOBB(position, rotation, scale);
        }

        public float GetRotation(Vector3 axis)
		{
            return axis.z * Mathf.Deg2Rad;
		}

        private Vector2[] UpdateVertices()
		{
            //Debug.Log("Update Vertices");
            Vector2[] vertices = new Vector2[4];
            vertices[0] = new Vector2(Min.x, Max.y);
            vertices[1] = new Vector2(Max.x, Max.y);
            vertices[2] = new Vector2(Max.x, Min.y);
			vertices[3] = new Vector2(Min.x, Min.y);

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
            MathU.Geometry.Transform transform = new MathU.Geometry.Transform(this.Position, this.Rotation);

            for (int i = 0; i < this.Vertices.Length; i++)
            {
                Vector2 v = this.Vertices[i];
                this.TransformedVertices[i] = VectorUtils.Transform(v-transform.Position, transform);
            }

            Debug.DrawLine(TransformedVertices[0], this.Position, Color.magenta);
            Debug.DrawLine(TransformedVertices[1], this.Position, Color.magenta);
            Debug.DrawLine(TransformedVertices[2], this.Position, Color.magenta);
            Debug.DrawLine(TransformedVertices[3], this.Position, Color.magenta);

            return this.TransformedVertices;
		}

        public void UpdateOBB(Vector2 position, Vector3 rotation, Vector2 scale)
        {
            this.Min = new Vector2(Position.x - scale.x / 2f, Position.y - scale.y / 2f);
            this.Max = new Vector2(Position.x + scale.x / 2f, Position.y + scale.y / 2f);
            
            this.Rotation = GetRotation(rotation);

            base.rotation = Quaternion.Euler(0, 0, Rotation);

            this.Vertices = UpdateVertices();

            this.TransformedVertices = GetTransformedVertices();

            base.requiresUpdate = false;
        }
    }
}
