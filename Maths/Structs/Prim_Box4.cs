﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yari.Maths.Structs
{

	public struct box4 : IDimension
	{

		public float x => xcentral - w / 2;
		public float y => ycentral - h / 2;
		public float xprom => xcentral + w / 2;
		public float yprom => ycentral + h / 2;
		public float xcentral { get; set; }
		public float ycentral { get; set; }
		public float w { get; set; }
		public float h { get; set; }

		public void LocateCentral(float x, float y)
		{
			xcentral = x;
			ycentral = y;
		}

		public void Locate(float x, float y)
		{
			LocateCentral(x + w / 2, y + h / 2);
		}

		public void Resize(float w, float h)
		{
			this.w = w;
			this.h = h;
		}

		public void Expand(float w, float h)
		{
			this.w += w;
			this.h += h;
		}

		public void Scale(float w, float h)
		{
			this.w *= w;
			this.h *= h;
		}

		public void Translate(float x, float y)
		{
			LocateCentral(this.xcentral + x, this.ycentral + y);
		}

		public bool Interacts(IDimension c)
		{
			return DoInteracts(x, y, w, h, c.x, c.y, c.w, c.h);
		}

		public bool Contains(float xi, float yi)
		{
			return xi >= x && xi <= x + w && yi >= y && yi <= y + h;
		}

		public bool Contains(vec2 vec)
		{
			return Contains(vec.x, vec.y);
		}

		public bool Contains(mutvec2 vec)
		{
			return Contains(vec.x, vec.y);
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

	}

	public struct rect4 : IDimension
	{

		public float x { get; set; }
		public float y { get; set; }
		public float w { get; set; }
		public float h { get; set; }
		public float xcentral => (x + xprom) / 2f;
		public float ycentral => (y + yprom) / 2f;
		public float xprom => x + w;
		public float yprom => y + h;

		public void Set(float x, float y, float w, float h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
		}

		public void Resize(float w, float h)
		{
			this.w = w;
			this.h = h;
		}

		public void Locate(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public void Translate(float x, float y)
		{
			Set(this.x + x, this.y + y, w, h);
		}

		public bool Interacts(IDimension c)
		{
			return DoInteracts(x, y, w, h, c.x, c.y, c.w, c.h);
		}

		public bool Contains(float xi, float yi)
		{
			return xi >= x && xi <= x + w && yi >= y && yi <= y + h;
		}

		public bool Contains(vec2 vec)
		{
			return Contains(vec.x, vec.y);
		}

		public bool Contains(mutvec2 vec)
		{
			return Contains(vec.x, vec.y);
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

	}

}
