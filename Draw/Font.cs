﻿using System;

namespace Yari.Draw
{

	public delegate int LocatePage(char ch);

	public class Font
	{

		public static Font load(Texture[] textures, LocatePage locator, int[] xa, int[] ya, int[] wa, int hf)
		{
			Font font = new Font();
			font.texture = textures;
			font.Locate = locator;
			font.GlyphX = xa;
			font.GlyphY = ya;
			font.GlyphWidth = wa;
			font.YSize = hf;

			return font;
		}

		public int[] GlyphWidth = new int[65536];
		public int[] GlyphX = new int[65536];
		public int[] GlyphY = new int[65536];

		public Texture[] texture;
		public int YSize;

		public LocatePage Locate;
		public float Scale = 1f;

		public GlyphBounds getBounds(string text, float maxWidth, bool useMinWidth)
		{
			int width = 0;
			int lineWidth = 0;
			int height = 0;
			int lineHeight = (int) (YSize * Scale);
			bool needNewLine = false;

			for(int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if(c == '\n' || needNewLine)
				{
					height += lineHeight;
					width = useMinWidth ? System.Math.Min(lineWidth, width) : System.Math.Max(lineWidth, width);
					lineWidth = 0;
					needNewLine = false;
					continue;
				}
				if(c == '\r')
				{
					continue;
				}
				if(lineWidth + GlyphWidth[c] * Scale >= maxWidth)
				{
					needNewLine = true;
					i -= 2;//correct index
					continue;
				}
				lineWidth += (int) (GlyphWidth[c] * Scale);
			}
			height += lineHeight;
			width = System.Math.Max(lineWidth, width);
			
			return new GlyphBounds(text, width, height);
		}

		public GlyphBounds getBounds(string text, float maxWidth)
		{
			return getBounds(text, maxWidth, false);
		}

		public GlyphBounds getBounds(string text)
		{
			return getBounds(text, int.MaxValue);
		}

	}

	public struct GlyphBounds
	{

		public string Sequence;
		public int Width;
		public int Height;

		public GlyphBounds(string sequence, int w, int h)
		{
			Sequence = sequence;
			Width = w;
			Height = h;
		}

	}

	public enum Align
	{

		LEFT,
		CENTER,
		RIGHT

	}

}