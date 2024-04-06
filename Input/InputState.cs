using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using Yari.Maths.Structs;

namespace Yari.Input
{

	public interface InputState
	{

		public rvec2 Cursor { get; }

		InputObserver Observe(Keycode code);

		InputObserver Observe(int code);

		string GetClipboardText();

		void PushToClipboard(string text);

		string GetTextInput();

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