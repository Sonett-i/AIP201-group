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

    public static void DrawShape(Geometry.Shapes shape, Vector3 position, Quaternion rotation, Vector3 scale, Color colour)
	{
        if (shape == Geometry.Shapes.Point)
		{
            GizmoTools.DrawCircleGizmo(position, rotation, 0.1f, colour);
        }
        else if (shape == Geometry.Shapes.Circle)
		{
            GizmoTools.DrawCircleGizmo(position, rotation, scale.x / 2f, colour);
		}
        else if (shape == Geometry.Shapes.AABB || shape == Geometry.Shapes.OBB)
		{
            GizmoTools.DrawRectangleGizmo(position, scale, rotation, colour);
		}
	}

	public static void DrawLine(Vector3 pointA, Vector3 pointB, Color colour)
	{
        Debug.DrawLine(pointB, pointA, colour);
	}
}