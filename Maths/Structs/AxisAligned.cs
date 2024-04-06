using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yari.Maths.Structs
{

	public class AxisAligned : IFourCoords
	{

		public float x { get; set; }
		public float y { get; set; }
		public float xprom { get; set; }
		public float yprom { get; set; }
		public float xcentral => (x + xprom) / 2f;
		public float ycentral => (y + yprom) / 2f;
		public float w => xprom - x;
		public float h => yprom - y;

		public bool Interacts(IFourCoords c)
		{
			return AxisAlignedSized.DoInteracts(x, y, w, h, c.x, c.y, c.w, c.h);
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
