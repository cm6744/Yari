using System.Collections.Generic;

namespace Yari.Maths.Structs
{

	public class rvec2
	{

		public float x, y;

		public rvec2()
		{
		}

		public rvec2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public rvec2(IReadOnlyList<float> rvec)
		{
			x = rvec[0];
			y = rvec[1];
		}

		public void Copy(rvec2 vec)
		{
			x = vec.x;
			y = vec.y;
		}

		public static rvec2 operator +(rvec2 rvec1, rvec2 rvec2)
		{
			rvec1.x += rvec2.x;
			rvec1.y += rvec2.y;
			return rvec1;
		}

		public static rvec2 operator +(rvec2 rvec1, vec2 vec2)
		{
			rvec1.x += vec2.x;
			rvec1.y += vec2.y;
			return rvec1;
		}

		public static rvec2 operator -(rvec2 rvec1, rvec2 rvec2)
		{
			rvec1.x -= rvec2.x;
			rvec1.y -= rvec2.y;
			return rvec1;
		}

		public static rvec2 operator -(rvec2 rvec1, vec2 vec2)
		{
			rvec1.x -= vec2.x;
			rvec1.y -= vec2.y;
			return rvec1;
		}

		public static rvec2 operator *(rvec2 rvec1, rvec2 rvec2)
		{
			rvec1.x *= rvec2.x;
			rvec1.y *= rvec2.y;
			return rvec1;
		}

		public static rvec2 operator *(rvec2 rvec1, vec2 vec2)
		{
			rvec1.x *= vec2.x;
			rvec1.y *= vec2.y;
			return rvec1;
		}

		public static rvec2 operator /(rvec2 rvec1, rvec2 rvec2)
		{
			rvec1.x /= rvec2.x;
			rvec1.y /= rvec2.y;
			return rvec1;
		}

		public static rvec2 operator /(rvec2 rvec1, vec2 vec2)
		{
			rvec1.x /= vec2.x;
			rvec1.y /= vec2.y;
			return rvec1;
		}

		public static rvec2 operator -(rvec2 rvec)
		{
			rvec.x = -rvec.x;
			rvec.y = -rvec.y;
			return rvec;
		}

		//Dot Operation.
		public static float operator ^(rvec2 rvec1, rvec2 rvec2)
		{
			return rvec1.x * rvec2.x + rvec1.y * rvec2.y;
		}

	}

}