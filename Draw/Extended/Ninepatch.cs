namespace Yari.Draw.Extended
{

	public class NinePatch : Icon
	{

		private Texture texture;
		private int l, r, t, b;
		public float Scale = 1;

		public NinePatch(Texture texture, int left, int right, int top, int bottom)
		{
			this.texture = texture;
			l = left;
			r = right;
			t = top;
			b = bottom;
		}

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			batch.Draw(texture, x, y, l * Scale, h, 0, 0, l, h / Scale);
			batch.Draw(texture, x, y, w, b * Scale, 0, texture.Height - b, w / Scale, b);
			batch.Draw(texture, x, y + h - t * Scale, w, t * Scale, 0, 0, w / Scale, t);
			batch.Draw(texture, x + w - r * Scale, y, r * Scale, h, texture.Width - r, 0, r, h / Scale);
			batch.Draw(texture, x + l * Scale, y + b * Scale, w - l * Scale - r * Scale, h - t * Scale - b * Scale, l,
				t, texture.Width - l - r, texture.Height - t - b);
		}

	}

}