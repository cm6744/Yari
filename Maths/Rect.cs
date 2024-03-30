namespace Yari.Maths
{

	public struct Rect
	{

		public float x;
		public float y;
		public float w;
		public float h;

		public Rect(float x, float y, float w, float h, bool center = false)
		{
			this.w = w;
			this.h = h;

			if(!center)
			{
				this.x = x;
				this.y = y;
			}
			else
			{
				this.x = x - w / 2;
				this.y = y - h / 2;
			}
		}

		public float xp => x + w;
		public float yp => y + w;
		public float xc => x + w / 2;
		public float yc => y + w / 2;

		public bool Interacts(Rect rect)
		{
			return Interacts(x, y, w, h, rect.x, rect.y, rect.w, rect.h);
		}

		public static bool Interacts(float x1, float y1, float width1, float height1, float x2, float y2, float width2,
			float height2)
		{
			width2 += x2;
			height2 += y2;
			width1 += x1;
			height1 += y1;

			return ((width2 < x2 || width2 > x1) && (height2 < y2 || height2 > y1)
			                                     && (width1 < x1 || width1 > x2) && (height1 < y1 || height1 > y2));
		}

		public bool Contains(float xi, float yi)
		{
			return xi >= x && xi <= x + w && yi >= y && yi <= y + h;
		}

		public bool Contains(vec2 vec)
		{
			return Contains(vec.x, vec.y);
		}

		public bool Contains(rvec2 vec)
		{
			return Contains(vec.x, vec.y);
		}

	}

}