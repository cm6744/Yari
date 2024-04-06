using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Yari.Audio;
using Yari.Codec;
using Yari.Common;
using Yari.Draw.Gui.Structs;
using Yari.Input;
using Yari.Maths.Structs;

namespace Yari.Draw.Gui
{

	public class Screen
	{

		public static Screen Viewing;
		public static Resolution Resolution;

		public Dictionary<int, Component> Components = new Dictionary<int, Component>();
		public Dictionary<int, BinaryCompound> DataStore = new Dictionary<int, BinaryCompound>();
		private List<int> ToRemove = new List<int>();
		public int DataIdNext;

		public vec2 Size;
		public float ScaleFactor;
		public Screen Parent;

		public virtual float ScaleMul => 1;
		public virtual float ScaleLocked => -1;

		public void Join(params Component[] components)
		{
			foreach(Component component in components)
			{
				if(DataStore.TryGetValue(DataIdNext, out BinaryCompound value))
				{
					component.PersistentData = value;
					component.ReloadData();
				}
				else
				{
					DataStore[DataIdNext] = component.PersistentData = new BinaryCompound();
				}

				Components[DataIdNext] = component;
				component.IdxInScreen = DataIdNext;
				DataIdNext++;
			}
		}

		public void Remove(Component component)
		{
			ToRemove.Add(component.IdxInScreen);
		}

		public void Remove(int idx)
		{
			ToRemove.Add(idx);
		}

		public void Reflush()
		{
			foreach(Component component in Components.Values)
			{
				component.SaveData();
			}
			Components.Clear();
			DataIdNext = 0;
			InitComponents();
		}

		public void Resolve(Resolution res)
		{
			Size = new vec2(res.ScaledWidth, res.ScaledHeight);
			ScaleFactor = res.ScaleFactor;

			Reflush();
		}

		public virtual void InitComponents() { }

		public Screen Extend(Screen scrp)
		{
			Parent = scrp;
			return this;
		}

		public void Display()
		{
			Viewing?.Close();
			Viewing = this;
			Resolution = new Resolution(this);
			Reflush();
		}

		public void Close()
		{
			OnClosed();
			Viewing = null;
		}

		public virtual void OnClosed() { }

		public virtual void Input(InputState state, rvec2 cursor)
		{
			foreach(Component comp in Components.Values)
			{
				comp.Input(state, cursor);
			}
		}

		public virtual void Update(TickSchedule schedule)
		{
			foreach(Component comp in Components.Values)
			{
				comp.Update(schedule);
			}

			foreach(int idx in ToRemove)
			{
				Components.Remove(idx);
				DataStore.Remove(idx);
			}
		}

		public virtual void Draw(DrawBatch batch)
		{
			foreach(Component comp in Components.Values)
			{
				comp.Draw(batch);
			}
		}

		public virtual AudioData[] MusicsAvailable => Array.Empty<AudioData>();

	}

}