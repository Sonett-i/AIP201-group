using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using PhysicsEngine;
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

		public static PhysicsEngine.PhysicsColliders.Collision IntersectCirclePolygon(Vector2 circleCenter, float circleRadius, Vector2[] vertices, bool flip = false)
		{
			PhysicsEngine.PhysicsColliders.Collision collision = new PhysicsEngine.PhysicsColliders.Collision();

			collision.normal = Vector2.zero;
			collision.intersection = float.MaxValue;

			Vector2 axis = Vector2.zero;
			float axisDepth = 0f;
			float minA, maxA, minB, maxB;

			for (int j = 0; j < vertices.Length; j++)
			{
				Vector2 vA = vertices[j]; // vertex A
				Vector2 vB = vertices[(j + 1) % vertices.Length]; // vertex B

				Vector2 edge = vB - vA;
				axis = new Vector2(-edge.y, edge.x);

				SAT.ProjectVertices(vertices, axis, out minA, out maxA);
				SAT.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);
				 
				if (minA >= maxB || minB >= maxA)
				{
					collision.Colliding = false;
					return collision;
				}

				axisDepth = Mathf.Min(maxB - minA, maxA - minB);

				if (axisDepth < collision.intersection)
				{
					collision.intersection = axisDepth;
					collision.normal = axis;
				}
			}

			int cpIndex = SAT.ClosestPointOnPolygon(circleCenter, vertices);
			Vector2 cp = vertices[cpIndex];
			axis = cp - circleCenter;

			SAT.ProjectVertices(vertices, axis, out minA, out maxA);
			SAT.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

			if (minA >= maxB || minB >= maxA)
			{
				collision.Colliding = false;
				return collision;
			}

			axisDepth = Mathf.Min(maxB - minA, maxA - minB);

			if (axisDepth < collision.intersection)
			{
				collision.intersection = axisDepth;
				collision.normal = axis;
			}

			collision.Colliding = true;
			collision.intersection /= collision.normal.magnitude;
			collision.normal = collision.normal.normalized;

			Vector2 centerB = Polygon.Centroid(vertices);

			Vector2 direction = (!flip) ? centerB - circleCenter : circleCenter - centerB;

			if (Vector2.Dot(direction, collision.normal) < 0f)
			{
				collision.normal = -collision.normal;
			}

			return collision;
		}

		private static int ClosestPointOnPolygon(Vector2 position, Vector2[] vertices)
		{
			int result = -1;
			float minDistance = float.MaxValue;

			for (int i = 0; i < vertices.Length; i++)
			{
				float d = (vertices[i] - position).magnitude;

				if (d < minDistance)
				{
					minDistance = d;
					result = i;
				}
			}

			return result;
		}
		public static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
		{
			Vector2 direction = axis.normalized;
			Vector2 directionAndRadius = direction * radius;

			Vector2 p1 = center + directionAndRadius;
			Vector2 p2 = center - directionAndRadius;

			min = Vector2.Dot(p1, axis);
			max = Vector2.Dot(p2, axis);

			if (min > max)
			{
				float t = min;
				min = max;
				max = t;
			}

		}

		public static void VisualizeSAT(Vector2[] verticesA, Vector2[] verticesB, int index)
		{
			for (int i = 0; i < 2; i++)
			{
				Vector2[] vertices = (i == 0) ? verticesA : verticesB;

				for (int j = 0; j < vertices.Length; j++)
				{
					Vector2 vA = vertices[j]; // vertex A
					Vector2 vB = vertices[(j + 1) % vertices.Length]; // vertex B

					Vector2 edge = vB - vA;
					Vector2 axis = new Vector2(-edge.y, edge.x);

					if (j == PhysicsEngine.Engine.PhysicsEngine.step)
					{
						Debug.DrawLine(vB, vA, Color.red);

						Debug.DrawLine(edge * 2, edge, Color.red);
						Debug.DrawLine(axis, vA, Color.cyan);
						Debug.DrawLine(axis, vB, Color.cyan);

						Debug.DrawLine(edge, axis, Color.cyan);
					}

					SAT.ProjectVertices(verticesA, axis, out float minA, out float maxA);
					SAT.ProjectVertices(verticesB, axis, out float minB, out float maxB);

				}
			}
		}

		public static PhysicsEngine.PhysicsColliders.Collision IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB)
		{
			PhysicsEngine.PhysicsColliders.Collision collision = new PhysicsEngine.PhysicsColliders.Collision();

			// iterate through both shapes
			for (int i = 0; i < 2; i++)
			{
				Vector2[] vertices = (i == 0) ? verticesA : verticesB;
				for (int j = 0; j < vertices.Length; j++)
				{
					Vector2 vA = vertices[j]; // polygon A
					Vector2 vB = vertices[(j + 1) % vertices.Length]; // polygon B, wraps around

					Vector2 edge = vB - vA;
					Vector2 axis = new Vector2(-edge.y, edge.x); // perpendicular to edge

					SAT.ProjectVertices(verticesA, axis, out float minA, out float maxA);
					SAT.ProjectVertices(verticesB, axis, out float minB, out float maxB);

					if (minA >= maxB || minB >= maxA)
					{
						collision.Colliding = false;
						return collision;
					}

					float axisDepth = Mathf.Min(maxB - minA, maxA - minB);
					if (axisDepth < collision.intersection)
					{
						collision.intersection = axisDepth;
						collision.normal = axis;
					}
				}
			}

			// If we've come this far, then there's intersection between the two shapes.
			collision.Colliding = true;
			collision.intersection /= collision.normal.magnitude;
			collision.normal = collision.normal.normalized;

			Vector2 centerA = Polygon.Centroid(verticesA);
			Vector2 centerB = Polygon.Centroid(verticesB);

			Vector2 direction = centerB - centerA;

			// Prevent tunneling if one shape is on other side.
			if (Vector2.Dot(direction, collision.normal) < 0f)
			{
				collision.normal = -collision.normal;
			}

			return collision;
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
			}
		}
	}
}
