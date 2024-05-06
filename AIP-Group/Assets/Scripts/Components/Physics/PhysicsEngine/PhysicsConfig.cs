using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsConfig : MonoBehaviour
{
	public static Vector3 Gravity = new Vector3(0, -9.81f, 0);
	public static float G = 6.67430e-11f; // gravitational constant

	public static Color CollidingColour = Color.red;
	public static Color DefaultDebugColour = Color.green;

	public static bool debugMode = true;
}
