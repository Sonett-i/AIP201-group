using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] float lineLength = 1f;
    static float _lineLength;


    public static void DrawPoint(Vector3 point, float radius, Color col)
    {
        Vector2[] points = new Vector2[2];
        points[0] = point;
        points[1] = (point - point) * radius;

        Debug.DrawLine(points[0], points[1], col);

    }

    float GetLineLength()
    {
        return lineLength;
    }

    public static Vector3 AxisIntersection(Vector3 vector, Axis axis, MinMax minmax)
    {
        Vector3 result = vector;

        float offset = (minmax == MinMax.max) ? _lineLength : -_lineLength;

        if (axis == Axis.x)
        {
            result.x += offset;
        }

        if (axis == Axis.y)
        {
            result.y += offset;
        }

        if (axis == Axis.z)
        {
            result.z += offset;
        }

        return result;
    }


    private void Update()
    {
        _lineLength = lineLength;
    }
}
