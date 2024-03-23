using System.Collections.Generic;
using Yari.Input;
using Yari.Math;

namespace Yari.Native.OpenGL
{

	public class GLInputState : InputState
	{

		public Dictionary<int, GLInputObserver> Observers = new Dictionary<int, GLInputObserver>();
		public string PileChars = "";
		public float Scroll;
		public vec2 Cursor = new vec2();

		public GLInputState()
		{
			InputState.Instance = this;
		}

		public InputObserver Observe(Keycode code)
		{
			return Observe((int) code);
		}

		public InputObserver Observe(int code)
		{
			if(!Observers.ContainsKey(code))
			{
				Observers[code] = new GLInputObserver();
			}

			GLInputObserver obs = Observers[code];

			return obs;
		}

		public string GetClipboardText()
		{
			return GraphicsDevice.Window.ClipboardString;
		}

		public void PushToClipboard(string text)
		{
			GraphicsDevice.Window.ClipboardString = text;
		}

		public string getTextInput()
		{
			return PileChars;
		}

		public void ConsumeTextInput()
		{
			PileChars = "";
		}

		public float GetCursorScroll()
		{
			return Mth.Abs(Scroll);
		}

		public void ConsumeCursorScroll()
		{
			Scroll = 0;
		}

		public ScrollDirection GetScrollDirection()
		{
			if(Scroll > 0)
			{
				return ScrollDirection.UP;
			}

			if(Scroll < 0)
			{
				return ScrollDirection.DOWN;
			}

			return ScrollDirection.NONE;
		}

		public vec2 GetCursor()
		{
			return Cursor;
		}

		public void StartRoll()
		{
		}

		public void EndRoll()
		{
			GLInputObserver.InputCheckTicks++;
		}

	}

}