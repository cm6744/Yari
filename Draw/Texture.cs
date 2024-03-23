namespace Yari.Draw
{

	public interface Icon
	{

		void Draw(DrawBatch batch, float x, float y, float w, float h);

	}

	public abstract class Texture : Icon
	{

		public int Width, Height;

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			batch.Draw(this, x, y, w, h);
		}

	}

	public struct TexturePart : Icon
	{

		public Texture Texture;
		public float u, v, uw, vh;

		public static TexturePart BySize(Texture tex, float u, float v, float uw, float vh)
		{
			TexturePart part;
			part.Texture = tex;
			part.u = u;
			part.v = v;
			part.uw = uw;
			part.vh = vh;
			return part;
		}

		public static TexturePart By(Texture tex)
		{
			return BySize(tex, 0, 0, tex.Width, tex.Height);
		}

		public static TexturePart ByVerts(Texture tex, float u, float v, float u2, float v2)
		{
			return BySize(tex, u, v, u2 - u, v2 - v);
		}

		public static TexturePart ByPercentSize(Texture tex, float u, float v, float w, float h)
		{
			return BySize(tex, tex.Width * u, tex.Height * v, tex.Width * w, tex.Width * h);
		}

		public static TexturePart ByPercentVerts(Texture tex, float u, float v, float u2, float v2)
		{
			return ByVerts(tex, tex.Width * u, tex.Height * v, tex.Width * u2, tex.Width * v2);
		}

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			batch.Draw(Texture, x, y, w, h, u, v, uw, vh);
		}

	}

}