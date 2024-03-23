using System;
using System.IO;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using Yari.Codec;
using Yari.Codec.General;
using Yari.Common.Manage;
using Yari.Draw;
using Yari.Math;

namespace Yari.Native.OpenGL
{

	public class GLShaderProgram : ShaderProgram
	{

		public int id;
		public Setup SetupDele;

		public GLShaderProgram(Setup setup)
		{
			SetupDele = setup;
			Reference.FREE.OnHoldReferred(Release);
		}

		public Attribute GetAttribute(string attr)
		{
			return new Attribute(GL.GetAttribLocation(id, attr));
		}

		public Uniform GetUniform(string name)
		{
			return new Uniform(GL.GetUniformLocation(id, name));
		}

		public override void Bind()
		{
			GL.UseProgram(id);
		}

		public override void Unbind()
		{
			GL.UseProgram(0);
		}

		public override void Setup()
		{
			SetupDele.Invoke(this);
		}

		public void Release()
		{
			Unbind();
			GL.DeleteProgram(id);
		}

	}

	public struct Attribute
	{

		private int id;

		public Attribute(int id)
		{
			this.id = (int)id;
		}

		public unsafe void PtrFloat(int size, int stride, int offset)
		{
			GL.VertexAttribPointer(id, size, VertexAttribPointerType.Float, false, 
				stride * sizeof(float), offset * sizeof(float));
		}

		public void Enable()
		{
			GL.EnableVertexAttribArray(id);
		}

	}

	public struct Uniform
	{

		private int id;
		
		public Uniform(int id)
		{
			this.id = id;
		}

		public unsafe void SetMat4(matrix4 mat4)
		{
			Matrix4x4 m4 = GLUtil.ToM4(mat4);
			GL.UniformMatrix4(id, 1, true, (float*) &m4);
		}

		public void Set1(double i)
		{
			GL.Uniform1(id, i);
		}

		public void Set2(double x, double y)
		{
			GL.Uniform2(id, x, y);
		}

		public void Set3(double x, double y, double z)
		{
			GL.Uniform3(id, x, y, z);
		}

		public void Set4(double x, double y, double z, double w)
		{
			GL.Uniform4(id, x, y, z, w);
		}

		public void SetTexUnit(int i, int unit)
		{
			GL.ActiveTexture(TextureUnit.Texture0 + unit);
			GL.BindTexture(TextureTarget.Texture2D, i);
			GL.Uniform1(id, unit);
			GL.ActiveTexture(TextureUnit.Texture0);
		}

	}

	public delegate void Setup(GLShaderProgram program);

	public class ShaderBuilds
	{

		public static GLShaderProgram Build(FileHandler fileNoSuffix, Setup shaderSetup)
		{
			string path = fileNoSuffix.Path;
			try
			{
				//if not using this way to read, it won't keep "\n", which shaders needs.
				string vert = StringIO.Read(new FileHandlerImpl(path + ".vert"));
				string frag = StringIO.Read(new FileHandlerImpl(path + ".frag"));
				return Build(vert, frag, shaderSetup);
			}
			catch(IOException)
			{
				throw new FileNotFoundException("Not found: " + path);
			}
		}

		public static GLShaderProgram Build(string vertex, string fragment, Setup shaderSetup)
		{
			GLShaderProgram shader = new GLShaderProgram(shaderSetup) { id = GL.CreateProgram() };

			int vert = BuildShaderPart(vertex, ShaderType.VertexShader);
			int frag = BuildShaderPart(fragment, ShaderType.FragmentShader);

			GL.AttachShader(shader.id, vert);
			GL.AttachShader(shader.id, frag);

			GL.BindFragDataLocation(shader.id, 0, "fragColor");

			GL.LinkProgram(shader.id);

			GL.DetachShader(shader.id, vert);
			GL.DetachShader(shader.id, frag);

			GL.ValidateProgram(shader.id);

			GL.DeleteShader(vert);
			GL.DeleteShader(frag);

			return shader;
		}

		private static int BuildShaderPart(string code, ShaderType type)
		{
			int id = GL.CreateShader(type);

			if(id == 0)
			{
				throw new Exception("Fail to create shader type :" + type);
			}

			GL.ShaderSource(id, code);
			GL.CompileShader(id);

			string txt = GL.GetShaderInfoLog(id);

			if(!string.IsNullOrEmpty(txt))
			{
				throw new Exception("GLERR => " + txt);
			}

			return id;
		}

	}

}