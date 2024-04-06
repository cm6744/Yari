namespace Yari.Draw
{

	public interface Icon
	{

		void Draw(DrawBatch batch, float x, float y, float w, float h);

	}

	public class IconNull : Icon
	{

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
		}

	}

	public abstract class Texture : Icon
	{

		public int Width, Height;

		public void Draw(DrawBatch batch, float x, float y, float w, float h)
		{
			batch.Draw(this, x, y, w, h);
		}

	}

}