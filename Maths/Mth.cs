namespace Yari.Maths
{

	public class Mth
	{

		private static readonly float[] SinTable = new float[0x10000];
		private const float DTR = (float) (System.Math.PI / 180F);
		private const float RTD = 1 / DTR;

		static Mth()
		{
			for(int i = 0; i < 0x10000; i++)
			{
				SinTable[i] = (float) System.Math.Sin((i * System.Math.PI * 2.0) / 65536.0);
			}
		}

		public static float Sqrt(float v)
		{
			return (float) System.Math.Sqrt(v);
		}

		public static float Log(float v)
		{
			return (float) System.Math.Log(v);
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
			return (float) System.Math.Atan2(y, x);
		}

		public static float AtanDeg(float y, float x)
		{
			return AtanRad(y, x) * RTD;
		}

		public static float SinRad(float v)
		{
			return SinTable[(int) (v * 10430.38F) & 0xffff];
		}

		public static float CosRad(float v)
		{
			return SinTable[(int) (v * 10430.38F + 16384F) & 0xffff];
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
			return v < min ? min : System.Math.Min(v, max);
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
			return (int) System.Math.Round(v);
		}

	}

}