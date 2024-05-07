using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;

public class PhysicsDebug : MonoBehaviour
{
    public enum Axis
    {
        x,
        y,
        z
    }

    public enum MinMax
    {
        min,
        max
    }


    // Drawing

    public static void DrawShape(Geometry.Shapes shape, Vector3 position, Quaternion rotation, Vector3 scale, Color colour, Vector2[] vertices = null)
	{
        if (shape == Geometry.Shapes.Circle)
		{
            GizmoTools.DrawCircleGizmo(position, rotation, scale.x / 2f, colour);
		}
        else if (shape == Geometry.Shapes.OBB)
		{
            GizmoTools.DrawRectangleGizmo(position, scale, rotation, colour);
		}
        else if (shape == Geometry.Shapes.Polygon)
		{
            if (vertices != null)
                DrawPolygon(vertices, colour);
		}
	}

	public static void DrawLine(Vector3 pointA, Vector3 pointB, Color colour)
	{
        if (PhysicsConfig.debugMode)
		{
            Debug.DrawLine(pointB, pointA, colour);
        }
	}

    public static void DrawPolygon(Vector2[] vertices, Color color)
	{
        for (int i = 0; i < vertices.Length; i++)
		{
            Debug.DrawLine(vertices[i], vertices[(i + 1) % vertices.Length], color);
            //DrawLine(vertices[i], vertices[(i+1) % vertices.Length], color);
		}
	}
}
