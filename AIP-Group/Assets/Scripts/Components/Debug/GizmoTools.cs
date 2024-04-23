using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmoTools
{
	public static void DrawCircleGizmo(Vector3 position, Quaternion rotation, float radius, Color colour)
	{
		Color oldColour = Gizmos.color;
		Gizmos.color = colour;

		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(position, rotation, new Vector3(1, 1, 0f));
		Gizmos.DrawWireSphere(Vector3.zero, radius);

		Gizmos.matrix = oldMatrix;
		Gizmos.color = oldColour;
	}

	public static void DrawRectangleGizmo(Vector3 position, Vector3 scale, Quaternion rotation, Color colour)
	{
		Color oldColour = Gizmos.color;
		Gizmos.color = colour;

		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(position, rotation, scale);
		Gizmos.DrawWireCube(Vector3.zero, scale);
	}
}
