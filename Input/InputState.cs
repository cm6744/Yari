using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using Yari.Maths;

namespace Yari.Input
{

	public interface InputState
	{

		public float[] Cursor { get; }

		InputObserver Observe(Keycode code);

		InputObserver Observe(int code);

		string GetClipboardText();

		void PushToClipboard(string text);

		string getTextInput();

		void ConsumeTextInput();

		float GetCursorScroll();

		void ConsumeCursorScroll();

		ScrollDirection GetScrollDirection();

		void StartRoll();

		void EndRoll();

	}

	public enum ScrollDirection
	{

		NONE,
		UP,
		DOWN

	}

}