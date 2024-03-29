using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Yari.Draw;
using Yari.Maths;

namespace Yari.Native.OpenGL
{

	public class GLGraphicEnv : GraphicEnv
	{

		public vec2 Size
		{
			get
			{
				unsafe
				{
					GLFW.GetWindowSize(GLDevice.Window, out int x, out int y);
					return new vec2(x, y);
				}
			}
		}

		public vec2 Pos 
		{
			get
			{
				unsafe
				{
					GLFW.GetWindowPos(GLDevice.Window, out int x, out int y);
					return new vec2(x, y);
				}
			}
		}

		public long Nanotime => (long) (GLFW.GetTime() * 1000_000_000);

	}

}