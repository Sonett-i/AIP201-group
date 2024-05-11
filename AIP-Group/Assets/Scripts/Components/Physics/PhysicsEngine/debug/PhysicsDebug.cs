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

    public static int step = 0;
    public static IEnumerator Step()
	{
        while (step < 3)
		{
            yield return new WaitForSeconds(1f);
            step++;
		}
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

    public static void DrawPolygon(Vector2[] vertices, Color colour)
	{
        for (int i = 0; i < vertices.Length; i++)
		{
            Debug.DrawLine(vertices[i], vertices[(i + 1) % vertices.Length], colour);
            //DrawLine(vertices[i], vertices[(i+1) % vertices.Length], color);
		}
	}

    public static void DrawProjectedAxis(Vector2 axis, Vector2[] vertices, Color color, int index = -1)
	{
        if (index != -1)
		{
            for (int i = 0; i < vertices.Length; i++)
            {

                Debug.DrawLine(vertices[i], axis, color);
            }
        }
        else
		{
            Debug.DrawLine(vertices[index], axis, color);
        }
            
	}
}
