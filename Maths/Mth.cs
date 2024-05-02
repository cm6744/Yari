﻿using System;

namespace Yari.Maths
{

	public class Mth
	{

		private const float DTR = (float) (Math.PI / 180F);
		private const float RTD = 1 / DTR;

		public static float Pow(float v, float d)
		{
			return (float) Math.Pow(v, d);
		}

		public static float Sqrt(float v)
		{
			return (float) Math.Sqrt(v);
		}

		public static float Log(float v)
		{
			return (float) Math.Log(v);
		}

		public static float Abs(float v)
		{
			return (v > 0 ? v : -v);
		}

		public static long Abs(long v)
		{
			return (v > 0 ? v : -v);
		}

		public static float AtanRad(float y, float x)
		{
			return (float) Math.Atan2(y, x);
		}

		public static float AtanDeg(float y, float x)
		{
			return AtanRad(y, x) * RTD;
		}

		public static float SinRad(float v)
		{
			return (float) Math.Sin(v);
		}

		public static float CosRad(float v)
		{
			return (float) Math.Cos(v);
		}

		public static float SinDeg(float v)
		{
			return SinRad(v * DTR);
		}

		public static float CosDeg(float v)
		{
			return CosRad(v * DTR);
		}

		public static float Rad(float v)
		{
			return v * DTR;
		}

		public static float Deg(float v)
		{
			return v * RTD;
		}

		public static float Clamp(float v, float min, float max)
		{
			return v < min ? min : Math.Min(v, max);
		}

		public static int Clamp(int v, int min, int max)
		{
			return v < min ? min : Math.Min(v, max);
		}

		public static int Factorial(int n)
		{
			return (n > 1) ? n * Factorial(n - 1) : 1;
		}

		public static int Permutations(int n, int m)
		{
			return (n >= m) ? Factorial(n) / Factorial(n - m) / Factorial(m) : 0;
		}

		public static int Floor(float v)
		{
			int i = (int) v;
			return v >= i ? i : i - 1;
		}

		public static int Round(float v)
		{
			return (int) Math.Round(v);
		}

	}

}