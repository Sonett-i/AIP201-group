using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MathU.Geometry
{
	public class Transform
	{
		public Vector2 Position;
		public float Sin;
		public float Cos;

		public static Transform Zero = new Transform(0, 0, 0f);

		public Transform(Vector2 position, float angle)
		{
			this.Position = position;
			this.Sin = Mathf.Sin(angle);
			this.Cos = Mathf.Cos(angle);
		}

		public Transform(float x, float y, float angle)
		{
			this.Position = new Vector2(x, y);
			this.Sin = Mathf.Sin(angle);
			this.Cos = Mathf.Cos(angle);
		}

	}
	public static class VectorUtils
	{
		// https://matthew-brett.github.io/teaching/rotation_2d.html

		/* x2 = cosβx1−sinβy1
		 * 
		 * y2 = sinβx1+cosβy1
		 */

		public static Vector3 Transform(Vector3 v, Transform transform)
		{
			return new Vector3(
				transform.Cos * v.x - transform.Sin * v.y + transform.Position.x,
				transform.Sin * v.x + transform.Cos * v.y + transform.Position.y);
		}
	}
}
