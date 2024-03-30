using Yari.Codec;
using Yari.Input;
using Yari.Maths;

namespace Yari.Draw.Gui.Structs
{

	public abstract class Component
	{

		public Rect Bound = new Rect();
		public BinaryCompound PersistentData = new BinaryCompound();
		public int IdxInScreen;

		public virtual void ReloadData() { }

		public virtual void SaveData() { }

		public virtual void Update() { }

		public virtual void Render(DrawBatch batch) { }

		public virtual void Input(InputState input, rvec2 cursor) { }

	}

}