﻿namespace Yari.Math
{
	public class Mthc
	{

		static readonly float[] BUF = new float[3];

		public static float[] HsvToRgb(float hue, float saturation, float value, float[] floats)
		{
			int i = (int) (hue * 6.0F) % 6;
			float f = hue * 6.0F - (float) i;
			float f1 = value * (1.0F - saturation);
			float f2 = value * (1.0F - f * saturation);
			float f3 = value * (1.0F - (1.0F - f) * saturation);
			float f4 = 0;
			float f5 = 0;
			float f6 = 0;

			switch(i)
			{
				case 0:
					f4 = value;
					f5 = f3;
					f6 = f1;
					break;
				case 1:
					f4 = f2;
					f5 = value;
					f6 = f1;
					break;
				case 2:
					f4 = f1;
					f5 = value;
					f6 = f3;
					break;
				case 3:
					f4 = f1;
					f5 = f2;
					f6 = value;
					break;
				case 4:
					f4 = f3;
					f5 = f1;
					f6 = value;
					break;
				case 5:
					f4 = value;
					f5 = f1;
					f6 = f2;
					break;
			}

			floats[0] = f4;
			floats[1] = f5;
			floats[2] = f6;

			return floats;
		}

		public static float[] HsvToRgb(float hue, float saturation, float value)
		{
			return HsvToRgb(hue, saturation, value, BUF);
		}

	}

}