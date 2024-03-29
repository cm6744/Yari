using System.Collections.Generic;

namespace Yari.Maths
{

	public struct vec2
	{

		public float x, y;

		public vec2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public vec2(IReadOnlyList<float> vec)
		{
			x = vec[0];
			y = vec[1];
		}

		public static vec2 operator +(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x + vec2.x, vec1.y + vec2.y);
		}

		public static vec2 operator -(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x - vec2.x, vec1.y - vec2.y);
		}

		public static vec2 operator *(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x * vec2.x, vec1.y * vec2.y);
		}

		public static vec2 operator /(vec2 vec1, vec2 vec2)
		{
			return new vec2(vec1.x / vec2.x, vec1.y / vec2.y);
		}

		public static vec2 operator -(vec2 vec)
		{
			return new vec2(-vec.x, -vec.y);
		}

		//Dot Operation.
		public static float operator ^(vec2 vec1, vec2 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y;
		}

	}

	public struct vec3
	{

		public float x, y, z;

		public vec3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public static vec3 operator +(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
		}

		public static vec3 operator -(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
		}

		public static vec3 operator *(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z);
		}

		public static vec3 operator /(vec3 vec1, vec3 vec2)
		{
			return new vec3(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z);
		}

		public static vec3 operator -(vec3 vec)
		{
			return new vec3(-vec.x, -vec.y, -vec.z);
		}

		//Dot Operation.
		public static float operator ^(vec3 vec1, vec3 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
		}

	}

	public struct vec4
	{

		public float x, y, z, w;

		public vec4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public static vec4 operator +(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
		}

		public static vec4 operator -(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z, vec1.w - vec2.w);
		}

		public static vec4 operator *(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x * vec2.x, vec1.y * vec2.y, vec1.z * vec2.z, vec1.w * vec2.w);
		}

		public static vec4 operator /(vec4 vec1, vec4 vec2)
		{
			return new vec4(vec1.x / vec2.x, vec1.y / vec2.y, vec1.z / vec2.z, vec1.w / vec2.w);
		}

		public static vec4 operator -(vec4 vec)
		{
			return new vec4(-vec.x, -vec.y, -vec.z, -vec.w);
		}

		//Dot Operation.
		public static float operator ^(vec4 vec1, vec4 vec2)
		{
			return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z + vec1.w * vec2.w;
		}

	}

}