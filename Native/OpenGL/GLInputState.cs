using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Yari.Input;
using Yari.Maths;

namespace Yari.Native.OpenGL
{

	public class GLInputState : InputState
	{

		public Dictionary<int, GLInputObserver> Observers = new Dictionary<int, GLInputObserver>();
		public string PileChars = "";
		public float Scroll;
		public float[] Cursor { get; } = new float[2];

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

		public unsafe string GetClipboardText()
		{
			return GLFW.GetClipboardString(GLDevice.Window);
		}

		public unsafe void PushToClipboard(string text)
		{
			GLFW.SetClipboardString(GLDevice.Window, text);
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

		public void StartRoll()
		{
		}

		public void EndRoll()
		{
			GLInputObserver.InputCheckTicks++;
		}

	}

}