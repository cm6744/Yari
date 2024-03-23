using System.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using Yari.Codec;
using Yari.Common.Manage;
using Texture = Yari.Draw.Texture;

namespace Yari.Native.OpenGL
{

	public class GLTexture : Texture
	{

		static GLTexture()
		{
			GL.Enable(EnableCap.Texture2D);
		}

		public int Id;
		
		public unsafe GLTexture(FileHandler handler)
		{
			Id = GL.GenTexture();
			
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Id);

			ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(handler.Path), ColorComponents.RedGreenBlueAlpha);

			Width = result.Width;
			Height = result.Height;

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				Width,
				Height,
				0,
				PixelFormat.Rgba,
				PixelType.UnsignedByte,
				result.Data
			);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) GraphicsDeviceSettings.FilterMag);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) GraphicsDeviceSettings.FilterMin);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) GraphicsDeviceSettings.Wrap);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) GraphicsDeviceSettings.Wrap);

			if(GraphicsDeviceSettings.MipmapLevel > 0)
			{
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, GraphicsDeviceSettings.MipmapLevel);
				GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			}

			Reference.FREE.OnHoldReferred(() => GL.DeleteTexture(Id));
		}

	}



}