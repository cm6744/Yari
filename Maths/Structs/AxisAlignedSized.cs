namespace Yari.Maths.Structs
{

	public struct AxisAlignedSized : IFourCoords
	{

		public float x { get; set; }
		public float y { get; set; }
		public float xprom => x + w;
		public float yprom => y + h;
		public float xcentral => (x + xprom) / 2f;
		public float ycentral => (y + yprom) / 2f;
		public float w { get; set; }
		public float h { get; set; }

		public AxisAlignedSized(float x, float y, float w, float h, bool center = false)
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

		public vec2 Size
		{
			get => new vec2(w, h);
			set
			{
				w = value.x;
				h = value.y;
			}
		}

		public vec2 Pos
		{
			get => new vec2(x, y);
			set
			{
				x = value.x;
				y = value.y;
			}
		}

		public vec2 PosCentral
		{
			get => new vec2(xcentral, ycentral);
			set
			{
				x = value.x - w / 2f;
				y = value.y - h / 2f;
			}
		}

		public bool Interacts(IFourCoords c)
		{
			return DoInteracts(x, y, w, h, c.x, c.y, c.w, c.h);
		}

		public static bool DoInteracts(float x1, float y1, float width1, float height1, float x2, float y2, float width2,
			float height2)
		{
			width2 += x2;
			height2 += y2;
			width1 += x1;
			height1 += y1;

			return (width2 < x2 || width2 > x1) && (height2 < y2 || height2 > y1)
												 && (width1 < x1 || width1 > x2) && (height1 < y1 || height1 > y2);
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