using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using Yari.Math;

namespace Yari.Input
{

	public interface InputState
	{

		public static InputState Instance;

		InputObserver Observe(Keycode code);

		InputObserver Observe(int code);

		string GetClipboardText();

		void PushToClipboard(string text);

		string getTextInput();

		void ConsumeTextInput();

		float GetCursorScroll();

		void ConsumeCursorScroll();

		ScrollDirection GetScrollDirection();

		vec2 GetCursor();

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