using System;
using System.ComponentModel;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Yari.Common.Manage;
using Yari.Draw;
using Yari.Math;
using Yari.Native.OpenAL;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace Yari.Native.OpenGL
{

	public class GraphicsDevice
	{

		public static GraphicsDeviceSettings Settings;
		public static int Pw, Ph;
		public static Lifecycle Lifecycle = new Lifecycle();
		public static GLNativeWindowBinding Window;
		public static GLInputState InputState;

		public static void LoadSettings(GraphicsDeviceSettings s)
		{
			Settings = s;
			Pw = (int) s.Size.x;
			Ph = (int) s.Size.y;
		}

		public static unsafe void Run(int tps)
		{
			NativeWindowSettings nSettings = new NativeWindowSettings
			{
				StartFocused = true,
				ClientSize = new Vector2i((int) Settings.Size.x, (int) Settings.Size.y),
				StencilBits = Settings.Stencil,
				NumberOfSamples = Settings.Samples,
				StartVisible = false
			};
			GameWindowSettings gSettings = new GameWindowSettings
			{
				UpdateFrequency = tps,

			};

			VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
			float x = (vm->Width - Settings.Size.x) / 2;
			float y = (vm->Height - Settings.Size.y) / 2;
			nSettings.Location = new Vector2i((int) x, (int) y);

			Window = new GLNativeWindowBinding(gSettings, nSettings);
			Window.IsVisible = true;
			Window.Run();
		}

		public static void OnUpdateFrame(double deltaDouble)
		{
			Lifecycle.Tps = (float) (1 / deltaDouble);
			Lifecycle.TaskTick.Invoke();

			ALAudioClip.CheckClipStates();
		}

		public static void OnRenderFrame(double deltaDouble)
		{
			float delta = (float) deltaDouble;
			Lifecycle.TaskRender.Invoke(delta);
			Lifecycle.Fps = (float) (1 / deltaDouble);
		}

		public static void OnResize(int x1, int y1)
		{
			Lifecycle.TaskResize.Invoke(Pw, Ph, x1, y1);
			Pw = x1;
			Ph = y1;
		}

		public static void OnClosed()
		{
			Reference.FREE.OnFinalized();
		}

		public static void OnLoad()
		{
			InputState = new GLInputState();
			Lifecycle.TaskLoad.Invoke();
		}

	}

	public class GLNativeWindowBinding : GameWindow
	{

		public GLNativeWindowBinding(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
		{
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			GraphicsDevice.OnLoad();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			GraphicsDevice.OnClosed();
		}

		public float TimePile;

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			TimePile += (float) args.Time;

			GraphicsDevice.OnRenderFrame(TimePile);

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

			TimePile = 0;

			GraphicsDevice.OnUpdateFrame(args.Time);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			GraphicsDevice.OnResize(e.Width, e.Height);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			int code = (int) e.Key;
			GLInputObserver obs = (GLInputObserver) GraphicsDevice.InputState.Observe(code);
			obs.Fire();
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);

			int code = (int) e.Key;
			GLInputObserver obs = (GLInputObserver) GraphicsDevice.InputState.Observe(code);
			obs.Consume();
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);

			int code = (int) e.Button;
			GLInputObserver obs = (GLInputObserver) GraphicsDevice.InputState.Observe(code);
			obs.Fire();
		}

		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			base.OnMouseUp(e);

			int code = (int) e.Button;
			GLInputObserver obs = (GLInputObserver) GraphicsDevice.InputState.Observe(code);
			obs.Consume();
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			base.OnMouseWheel(e);

			GraphicsDevice.InputState.Scroll = e.OffsetY;
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			base.OnMouseMove(e);

			GraphicsDevice.InputState.Cursor = new vec2(e.X, e.Y);
		}

	}

}