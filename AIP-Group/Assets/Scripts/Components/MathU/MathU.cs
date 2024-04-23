using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathU
{
	public static float Clamp(float value, float min, float max)
	{
		if (min > max)
			Debug.LogError("Minimum value cannot be greater than maximum value.");

		return value < min ? min : value > max ? max : value;
	}
}
