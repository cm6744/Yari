using System;
using OpenTK.Graphics.OpenGL;
using Yari.Common;
using Yari.Draw;
using Yari.Maths;
using Yari.Maths.Structs;
using Texture = Yari.Draw.Texture;

namespace Yari.Native.OpenGL
{

	public class GLFreeTessellator : FreeTessellator
	{

		public float[] Vertices;
		public int NumVertices, Idx;

		public VertArrayObject<float, int> Vao;
		public BufferObject<float> Vbo;

		private PrimitiveType NowType;

		public int MinVertexBufSize = 256;

		public GLFreeTessellator(int size)
		{
			Vertices = new float[size];

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			Vbo = new BufferObject<float>(Vertices, BufferTarget.ArrayBuffer);
			Vao = new VertArrayObject<float, int>(Vbo, null);
		}

		public void Load(GLShaderProgram program)
		{
			Flush();

			Vao.Bind();

			Program = program;
			program.Bind();
			program.Setup();
		}

		public override void PrimitiveMode(Primitives primitives)
		{
			
			Flush();

			NowType = primitives switch
			{
				Primitives.Triangles => PrimitiveType.Triangles,
				Primitives.Lines => PrimitiveType.Lines,
				Primitives.Point => PrimitiveType.Points,
				_ => PrimitiveType.Triangles
			};
		}

		public override void UseShader(ShaderProgram program)
		{
			Load((GLShaderProgram) program);
		}

		public override void CheckTransformAndCap()
		{
			if(Vertices.Length - Idx < MinVertexBufSize)
			{
				Flush();
			}

			if(Matrices.Changed)
			{
				Transform.Set(Matrices.Top);
				Matrices.Changed = false;
			}
		}

		public override void Write(float v)
		{
			Vertices[Idx++] = v;
		}

		public override void Write(vec2 vec)
		{
			Vertices[Idx++] = vec.x;
			Vertices[Idx++] = vec.y;
		}

		public override void Write(vec3 vec)
		{
			Vertices[Idx++] = vec.x;
			Vertices[Idx++] = vec.y;
			Vertices[Idx++] = vec.z;
		}

		public override void Write(vec4 vec)
		{
			Vertices[Idx++] = vec.x;
			Vertices[Idx++] = vec.y;
			Vertices[Idx++] = vec.z;
			Vertices[Idx++] = vec.w;
		}

		public override void Write(params float[] arr)
		{
			Array.Copy(arr, 0, Vertices, Idx, arr.Length);
			Idx += arr.Length;
		}

		public override void WriteTransformed(vec2 vec)
		{
			vec2 vecTrf = Transform.ApplyTo(ref vec);
			Vertices[Idx++] = vecTrf.x;
			Vertices[Idx++] = vecTrf.y;
		}

		public override void NewVertex(int v)
		{
			NumVertices += v;
		}

		public override void Flush()
		{
			if(NumVertices <= 0)
			{
				return;
			}

			Program.Bind();

			Vao.Bind();
			Vbo.Bind();
			Vbo.UpdateBuffer(0, Vertices);

			GL.DrawArrays(NowType, 0, (int) NumVertices);

			Program.Unbind();

			NumVertices = 0;
			Idx = 0;
		}

	}

}