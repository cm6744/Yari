using System.Drawing;
using System;
using Yari.Maths;

namespace Yari.Draw
{

	public abstract class DrawBatch
	{

		public affine Transform = new affine();
		public matrix4 Projection = new matrix4();
		public MatrixStack Matrices = new MatrixStack();
		public bool FlipX;
		public bool FlipY;
		public vec4[] Color = new vec4[4];
		public int DrawCalls = 0;
		public vec4 ViewportArray;
		public Font Font;

		public abstract void Draw(Texture texture, float x, float y, float width, float height, float srcX, float srcY,
			float srcWidth, float srcHeight);

		public void Draw(Texture tex, float x, float y, float sx, float sy, float sw, float sh)
		{
			Draw(tex, x, y, sw, sh, sx, sy, sw, sh);
		}

		public void Draw(Texture tex, float x, float y, float w, float h)
		{
			Draw(tex, x, y, w, h, 0, 0, tex.Width, tex.Height);
		}

		public void Draw(Texture tex, float x, float y)
		{
			Draw(tex, x, y, tex.Width, tex.Height, 0, 0, tex.Width, tex.Height);
		}

		public void Draw(Texture tex, Rect rect, float sx, float sy, float sw, float sh)
		{
			Draw(tex, rect.x, rect.y, rect.w, rect.h, sx, sy, sw, sh);
		}

		public void Draw(Texture tex, Rect rect)
		{
			Draw(tex, rect.x, rect.y, rect.w, rect.h);
		}

		public void Draw(Icon icon, float x, float y, float w, float h)
		{
			icon.Draw(this, x, y, w, h);
		}

		public void Draw(Icon icon, Rect rect)
		{
			Draw(icon, rect.x, rect.y, rect.w, rect.h);
		}

		public abstract void Fill(float x, float y, float width, float height);

		//This method use the textured mode to present a rectangle.
		//Sometimes this is a much better choice.
		public abstract void FillTex(float x, float y, float width, float height);

		//Invoke this before you start each object rendering with your own write verts.
		public abstract void CheckTransformAndCap();

		public abstract void Write(float v);

		public abstract void Write(vec2 vec);

		public abstract void Write(vec3 vec);

		public abstract void Write(vec4 vec);

		public abstract void Write(params float[] arr);

		public abstract void WriteTransformed(vec2 vec);

		public abstract void NewVertex(int v);

		public abstract void PrimitiveMode(Primitives primitives);

		public abstract void UseShader(ShaderProgram program);

		public abstract void FragMode(Frags mode);

		public abstract void Flush();

		public void Color4(float r, float g, float b, float a)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(r, g, b, a);
		}

		public void Color4(vec3 vec3, float a)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(vec3.x, vec3.y, vec3.z, a);
		}

		public void Color4(vec3 vec3)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(vec3.x, vec3.y, vec3.z, 1);
		}

		public void Color4(vec4 color)
		{
			Color[0] = Color[1] = Color[2] = Color[3] = color;
		}

		public void Merge4(float r, float g, float b, float a)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= new vec4(r, g, b, a);
		}

		public void Merge4(vec3 vec3, float a)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= new vec4(vec3.x, vec3.y, vec3.z, a);
		}

		public void Merge4(vec3 vec3)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= new vec4(vec3.x, vec3.y, vec3.z, 1);
		}

		public void Merge4(vec4 color)
		{
			Color[0] = Color[1] = Color[2] = Color[3] *= color;
		}

		public void NormalizeColor()
		{
			Color[0] = Color[1] = Color[2] = Color[3] = new vec4(1, 1, 1, 1);
		}

		public abstract void Viewport(vec4 viewport);
		public abstract void Viewport(float x, float y, float w, float h);
		public abstract void Scissor(PerspectiveCamera camera, vec4 viewport, float x, float y, float w, float h);
		public abstract void ScissorEnd();
		public abstract void Clear();
		public abstract void UseCamera(PerspectiveCamera camera);

		//Font Rendering:

		public void Draw(string text, float x, float y, float maxWidth)
		{
			int fontHeight = (int) (Font.YSize * Font.Scale);

			float drawX = x;
			float drawY = y;

			bool newLine = false;

			for(int i = 0; i < text.Length; i++)
			{
				char ch = text[i];
				if(ch == '\n' || newLine)
				{
					drawY -= fontHeight;
					drawX = x;
					newLine = false;
					continue;
				}

				if(ch == '\r')
				{
					continue;
				}

				int w = (int) (Font.GlyphWidth[ch] * Font.Scale);
				if(drawX - x + w >= maxWidth)
				{
					newLine = true;
					i -= (int) (2 * Font.Scale); //correct index
					continue;
				}

				Draw(Font.texture[Font.Locate(ch)], drawX, drawY, w, fontHeight,
					Font.GlyphX[ch], Font.GlyphY[ch], Font.GlyphWidth[ch], Font.YSize);
				drawX += w;
			}
		}

		public void Draw(string text, float x, float y, Align align)
		{
			GlyphBounds bounds = Font.getBounds(text);

			switch(align)
			{
				case Align.LEFT:
					Draw(text, x, y, int.MaxValue);
					break;
				case Align.RIGHT:
					Draw(text, (x - bounds.Width), y, int.MaxValue);
					break;
				case Align.CENTER:
					Draw(text, (x - bounds.Width / 2F), y, int.MaxValue);
					break;
			}
		}

	}

	public enum Primitives
	{

		Triangles,
		Lines,
		Point

	}

	public enum Frags
	{

		Textured,
		ColorFill

	}

}