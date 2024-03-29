using System;
using System.ComponentModel;
using System.Runtime;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Yari.Audio;
using Yari.Common;
using Yari.Common.Manage;
using Yari.Common.Toolkit;
using Yari.Draw;
using Yari.Maths;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Yari.Native.OpenGL
{

	public class GLDevice
	{

		public static unsafe Window* Window;

		public static GraphicsDeviceSettings Settings;
		public static int Pw, Ph;
		
		public static void LoadSettings(GraphicsDeviceSettings s)
		{
			Settings = s;
			Pw = (int) s.Size.x;
			Ph = (int) s.Size.y;
		}

		public static unsafe void OpenWindow()
		{
			GLFW.Init();
			GLFW.SwapInterval(1);
			GLFW.DefaultWindowHints();
			GLFW.WindowHint(WindowHintBool.Decorated, Settings.Decorated);
			GLFW.WindowHint(WindowHintBool.Floating, Settings.Floating);
			GLFW.WindowHint(WindowHintBool.Resizable, Settings.Resizable);
			GLFW.WindowHint(WindowHintBool.Maximized, Settings.Maximized);
			GLFW.WindowHint(WindowHintBool.AutoIconify, Settings.AutoIconify);
			GLFW.WindowHint(WindowHintBool.FocusOnShow, true);
			GLFW.WindowHint(WindowHintBool.Visible, false);
			
			Window = GLFW.CreateWindow(Pw, Ph, Settings.Title, null, null);

			VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
			float x = (vm->Width - Settings.Size.x) / 2;
			float y = (vm->Height - Settings.Size.y) / 2;
			GLFW.SetWindowPos(Window, (int) x, (int) y);

			if(Settings.Maximized) GLFW.MaximizeWindow(Window);

			GLFW.MakeContextCurrent(Window);
			GLFW.ShowWindow(Window);
			GL.LoadBindings(new GLFWBindingsContext());

			OnLoad();

			Log.Info("GLFW Window is created, and GL Lib is loaded.");
		}

		public static unsafe void OnLoad()
		{
			Platform.GraphicEnv = new GLGraphicEnv();
			Platform.InputState = new GLInputState();

			Platform.Lifecycle.TaskTick += GLFW.PollEvents;

			GLFW.SetWindowCloseCallback(Window, (_) =>
			{
				Platform.IsExited = true;
			});
			GLFW.SetCharCallback(Window, (_, codepoint) =>
			{
				GLInputState state = (GLInputState) Platform.InputState;
				state.PileChars += (char) codepoint;
			});
			GLFW.SetKeyCallback(Window, (_, key, code, action, mods) =>
			{
				GLInputState state = (GLInputState) Platform.InputState;
				GLInputObserver observer = (GLInputObserver) state.Observe((int) key);
				switch (action)
				{
					case InputAction.Press:
					case InputAction.Repeat:
						observer.Fire();
						break;
					case InputAction.Release:
						observer.Consume();
						break;
				}
			});
			GLFW.SetMouseButtonCallback(Window, (_, key, action, mods) =>
			{
				GLInputState state = (GLInputState) Platform.InputState;
				GLInputObserver observer = (GLInputObserver) state.Observe((int) key);
				switch(action)
				{
					case InputAction.Press:
					case InputAction.Repeat:
						observer.Fire();
						break;
					case InputAction.Release:
						observer.Consume();
						break;
				}
			});
			GLFW.SetCursorPosCallback(Window, (_, x, y) =>
			{
				GLInputState state = (GLInputState) Platform.InputState;
				state.Cursor[0] = (float) x;
				state.Cursor[1] = (float) (Platform.GraphicEnv.Size.y - y);
			});
			GLFW.SetScrollCallback(Window, (_, x, y) =>
			{
				GLInputState state = (GLInputState) Platform.InputState;
				state.Scroll = (float) y;
			});
			GLFW.SetWindowSizeCallback(Window, (_, width, height) =>
			{
				Platform.Lifecycle.TaskResize.Invoke(Pw, Ph, width, height);
				Pw = width;
				Ph = height;
			});
		}

	}

}