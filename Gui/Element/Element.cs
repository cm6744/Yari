using Yari.Codec;
using Yari.Draw;
using Yari.Input;
using Yari.Math;

namespace Yari.Gui.Element
{

	public class Element
	{

		public Rect Bound = new Rect();
		public BinaryCompound Compound = new BinaryCompound();

		public void ReloadData()
		{
		}

		public void Update()
		{
		}

		public void Render(DrawBatch batch)
		{
		}

		public void Input(InputState input, vec2 cursor)
		{
		}

	}

}