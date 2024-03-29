using System.Collections.Generic;
using Yari.Codec;
using Yari.Common;
using Yari.Draw.Gui.Structs;
using Yari.Maths;

namespace Yari.Draw.Gui
{

	public class Screen
	{

		public static Screen Viewing;

		public Dictionary<int, Component> Components = new Dictionary<int, Component>();
		public Dictionary<int, BinaryCompound> DataStore = new Dictionary<int, BinaryCompound>();
		public int DataIdNext;

		public vec2 Size;
		public float[] Cursor;
		public float ScaleFactor;

		public virtual float ScaleMul => 1;
		public virtual float ScaleLocked => 0;

		public void Join(Component component)
		{
			if(DataStore.TryGetValue(DataIdNext, out BinaryCompound value))
			{
				component.PersistentData = value;
			}
			else
			{
				DataStore[DataIdNext] = component.PersistentData = new BinaryCompound();
			}

			Components[DataIdNext] = component;
			component.IdxInScreen = DataIdNext;
			DataIdNext++;
		}

		public void Remove(Component component)
		{
			Components.Remove(component.IdxInScreen);
			DataStore.Remove(component.IdxInScreen);
		}

		public void Remove(int idx)
		{
			Components.Remove(idx);
			DataStore.Remove(idx);
		}

		public void Reflush()
		{
			Components.Clear();
			DataIdNext = 0;
			InitComponents();
		}

		public void Resolve(Resolution res, float[] cursor)
		{
			Size = new vec2(res.ScaledWidth, res.ScaledHeight);
			Cursor = cursor;
			ScaleFactor = res.ScaleFactor;

			Reflush();
		}

		public virtual void InitComponents() {}

		public void Display()
		{
			Viewing?.Close();
			Reflush();
		}

		public void Close()
		{
			OnClosed();
			Viewing = null;
		}

		public virtual void OnClosed() { }

	}

}