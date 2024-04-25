using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Math Utilities

namespace MathU
{
	public static class Piecewise
	{
		public static float Clamp(float value, float min, float max)
		{
			if (min > max)
				Debug.LogError("Minimum value cannot be greater than maximum value.");

			return value < min ? min : value > max ? max : value;
		}
	}
}