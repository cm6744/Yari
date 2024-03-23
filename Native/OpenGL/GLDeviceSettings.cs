﻿using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Yari.Math;

namespace Yari.Native.OpenGL
{

	public class GraphicsDeviceSettings
	{

		public vec2 Size = new vec2(128, 128);
		public bool Fullframe;
		
		public int Stencil = 1;
		public int Samples = 1;
		public string Title = "Dimcube - OpenGL (Silk.NET)";
		public Dictionary<string, object> Properties = new Dictionary<string, object>();
		public GLTexture Icon;
		public GLTexture Cursor;
		public vec2 Hotspot = new vec2(0, 0);

		public static int MipmapLevel = 0;
		public static TextureMagFilter FilterMag = TextureMagFilter.Nearest;
		public static TextureMinFilter FilterMin = TextureMinFilter.Nearest;
		public static TextureWrapMode Wrap = TextureWrapMode.Repeat;

		public vec4 ClearColor = new vec4(0, 0, 0, 1);

	}

}