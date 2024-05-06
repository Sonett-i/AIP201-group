using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PhysicsEngine.PhysicsColliders;

namespace MathU.Geometry
{
	internal class SAT
	{
		public enum axis
		{
			x, 
			y, 
			z,
		}

		public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB)
		{
			//Debug.Log(verticesA.Length + " " + verticesB.Length);
			for (int i = 0; i < 2; i++)
			{
				Vector2[] vertices = (i == 0) ? verticesA : verticesB;

				for (int j = 0; j < vertices.Length; j++)
				{
					Vector2 vA = vertices[j]; // vertex A
					Vector2 vB = vertices[(j + 1) % vertices.Length]; // vertex B

					Vector2 edge = vB - vA;
					Vector2 axis = new Vector2(-edge.y, edge.x);

					SAT.ProjectVertices(verticesA, axis, out float minA, out float maxA);
					SAT.ProjectVertices(verticesB, axis, out float minB, out float maxB);

					if (minA >= maxB || minB >= maxA)
					{
						return false;
					}
				}
			}

			return true;
		}

		public static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
		{
			min = float.MaxValue;
			max = float.MinValue;

			for (int i = 0; i < vertices.Length; i++)
			{
				Vector2 v = vertices[i];
				
				float projection = Vector2.Dot(v, axis);

				if (projection < min) { min = projection; }
				if (projection > max) { max = projection; }

				if (PhysicsConfig.debugMode)
				{

				}
				PhysicsDebug.DrawLine(v, axis, Color.magenta);
			}
		}
	}
}
