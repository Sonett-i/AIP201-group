using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
			for (int i = 0; i < 2; i++)
			{
				Vector2[] vertices = (i == 0) ? verticesA : verticesB;

				for (int j = 0; j < vertices.Length; j++)
				{
					Vector2 vA = verticesA[j]; // vertex A
					Vector2 vB = verticesA[(j + 1) % verticesA.Length]; // vertex B

					Vector2 edge = vB - vA;
					Vector2 axis = new Vector2(-edge.y, edge.x);

					ProjectVertices(verticesA, axis, out float minA, out float maxA);
					ProjectVertices(verticesB, axis, out float minB, out float maxB);

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
				Debug.DrawLine(v, axis, Color.magenta);
				float projection = Vector2.Dot(v, axis);

				if (projection < min) { min = projection; }
				if (projection > max) { max = projection; }
			}
		}
	}
}
