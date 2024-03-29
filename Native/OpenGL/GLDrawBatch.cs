using System;
using OpenTK.Graphics.OpenGL;
using Yari.Common;
using Yari.Draw;
using Yari.Maths;
using Texture = Yari.Draw.Texture;

namespace Yari.Native.OpenGL
{

	public class GLDrawBatch : DrawBatch
	{

		public float[] Vertices;
		public int NumVertices, Idx;

		public VertArrayObject<float, int> Vao;
		public BufferObject<float> Vbo;

		public GLShaderProgram Program;
		public GLShaderProgram ProgramDefault;
		public GLShaderProgram ProgramShape;

		Uniform UniTexture;
		Uniform UniProjection;

		int TextureID;
		float InvTexWidth;
		float InvTexHeight;

		private PrimitiveType NowType;

		public GLTexture TextureColorFill;
		public int MinVertexBufSize = 256;

		public GLDrawBatch(int size)
		{
			NormalizeColor();
			Vertices = new float[size];

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			Vbo = new BufferObject<float>(Vertices, BufferTarget.ArrayBuffer);
			Vao = new VertArrayObject<float, int>(Vbo, null);

			ProgramDefault = GetDefaultShader();
			ProgramShape = GetDefaultShaderShape();

			Load(ProgramDefault);

			UniTexture = ProgramDefault.GetUniform("u_texture");

			//Get as default
			ViewportArray = new vec4(0, 0, Platform.GraphicEnv.Size.x, Platform.GraphicEnv.Size.y);
		}

		public void Load(GLShaderProgram program)
		{
			Flush();

			Vao.Bind();

			Program = program;
			program.Bind();
			program.Setup();

			UniProjection = program.GetUniform("u_proj");
			UniProjection.SetMat4(Projection);
		}

		public void Unload()
		{
			Load(ProgramDefault);
		}

		public override void PrimitiveMode(Primitives primitives)
		{
			Flush();

			NowType = primitives switch
			{
				Primitives.Triangles => PrimitiveType.Triangles,
				Primitives.Lines => PrimitiveType.Lines,
				Primitives.Point => PrimitiveType.Points
			};
		}

		public override void UseShader(ShaderProgram program)
		{
			Load((GLShaderProgram) program);
		}

		public override void FragMode(Frags mode)
		{
			Flush();

			switch(mode)
			{
				case Frags.Textured:
					Load(ProgramDefault);
					break;
				case Frags.ColorFill:
					Load(ProgramShape);
					break;
			}
		}

		public override void Draw(Texture texture, float x, float y, float width, float height, float srcX, float srcY,
			float srcWidth, float srcHeight)
		{
			GLTexture glt = (GLTexture) texture;

			CheckTransformAndCap();

			int id = glt.Id;

			if(id != TextureID)
			{
				InvTexWidth = 1f / texture.Width;
				InvTexHeight = 1f / texture.Height;
				Flush();
			}

			TextureID = id;

			float x1 = x + Transform.m02;
			float y1 = y + Transform.m12;
			float x2 = x + Transform.m01 * height + Transform.m02;
			float y2 = y + Transform.m11 * height + Transform.m12;
			float x3 = x + Transform.m00 * width + Transform.m01 * height + Transform.m02;
			float y3 = y + Transform.m10 * width + Transform.m11 * height + Transform.m12;
			float x4 = x + Transform.m00 * width + Transform.m02;
			float y4 = y + Transform.m10 * width + Transform.m12;

			float u = (srcX) * InvTexWidth;
			float v = (srcY) * InvTexHeight;
			float u2 = (srcX + srcWidth) * InvTexWidth;
			float v2 = (srcY + srcHeight) * InvTexHeight;

			if(FlipX)
			{
				(u, u2) = (u2, u);
			}

			if(FlipY)
			{
				(v, v2) = (v2, v);
			}

			//RT
			Vertices[Idx++] = (x3);
			Vertices[Idx++] = (y3);
			Vertices[Idx++] = (Color[2].x);
			Vertices[Idx++] = (Color[2].y);
			Vertices[Idx++] = (Color[2].z);
			Vertices[Idx++] = (Color[2].w);
			Vertices[Idx++] = (u2);
			Vertices[Idx++] = (v);

			//RD
			Vertices[Idx++] = (x4);
			Vertices[Idx++] = (y4);
			Vertices[Idx++] = (Color[3].x);
			Vertices[Idx++] = (Color[3].y);
			Vertices[Idx++] = (Color[3].z);
			Vertices[Idx++] = (Color[3].w);
			Vertices[Idx++] = (u2);
			Vertices[Idx++] = (v2);

			//LT
			Vertices[Idx++] = (x2);
			Vertices[Idx++] = (y2);
			Vertices[Idx++] = (Color[1].x);
			Vertices[Idx++] = (Color[1].y);
			Vertices[Idx++] = (Color[1].z);
			Vertices[Idx++] = (Color[1].w);
			Vertices[Idx++] = (u);
			Vertices[Idx++] = (v);

			//RD
			Vertices[Idx++] = (x4);
			Vertices[Idx++] = (y4);
			Vertices[Idx++] = (Color[3].x);
			Vertices[Idx++] = (Color[3].y);
			Vertices[Idx++] = (Color[3].z);
			Vertices[Idx++] = (Color[3].w);
			Vertices[Idx++] = (u2);
			Vertices[Idx++] = (v2);

			//LD
			Vertices[Idx++] = (x1);
			Vertices[Idx++] = (y1);
			Vertices[Idx++] = (Color[0].x);
			Vertices[Idx++] = (Color[0].y);
			Vertices[Idx++] = (Color[0].z);
			Vertices[Idx++] = (Color[0].w);
			Vertices[Idx++] = (u);
			Vertices[Idx++] = (v2);

			//LT
			Vertices[Idx++] = (x2);
			Vertices[Idx++] = (y2);
			Vertices[Idx++] = (Color[1].x);
			Vertices[Idx++] = (Color[1].y);
			Vertices[Idx++] = (Color[1].z);
			Vertices[Idx++] = (Color[1].w);
			Vertices[Idx++] = (u);
			Vertices[Idx++] = (v);

			NewVertex(6);
		}

		public override void Fill(float x, float y, float width, float height)
		{
			CheckTransformAndCap();

			float x1 = x + Transform.m02;
			float y1 = y + Transform.m12;
			float x2 = x + Transform.m01 * height + Transform.m02;
			float y2 = y + Transform.m11 * height + Transform.m12;
			float x3 = x + Transform.m00 * width + Transform.m01 * height + Transform.m02;
			float y3 = y + Transform.m10 * width + Transform.m11 * height + Transform.m12;
			float x4 = x + Transform.m00 * width + Transform.m02;
			float y4 = y + Transform.m10 * width + Transform.m12;

			//RT
			Vertices[Idx++] = (x3);
			Vertices[Idx++] = (y3);
			Vertices[Idx++] = (Color[2].x);
			Vertices[Idx++] = (Color[2].y);
			Vertices[Idx++] = (Color[2].z);
			Vertices[Idx++] = (Color[2].w);

			//RD
			Vertices[Idx++] = (x4);
			Vertices[Idx++] = (y4);
			Vertices[Idx++] = (Color[3].x);
			Vertices[Idx++] = (Color[3].y);
			Vertices[Idx++] = (Color[3].z);
			Vertices[Idx++] = (Color[3].w);

			//LT
			Vertices[Idx++] = (x2);
			Vertices[Idx++] = (y2);
			Vertices[Idx++] = (Color[1].x);
			Vertices[Idx++] = (Color[1].y);
			Vertices[Idx++] = (Color[1].z);
			Vertices[Idx++] = (Color[1].w);

			//RD
			Vertices[Idx++] = (x4);
			Vertices[Idx++] = (y4);
			Vertices[Idx++] = (Color[3].x);
			Vertices[Idx++] = (Color[3].y);
			Vertices[Idx++] = (Color[3].z);
			Vertices[Idx++] = (Color[3].w);

			//LD
			Vertices[Idx++] = (x1);
			Vertices[Idx++] = (y1);
			Vertices[Idx++] = (Color[0].x);
			Vertices[Idx++] = (Color[0].y);
			Vertices[Idx++] = (Color[0].z);
			Vertices[Idx++] = (Color[0].w);

			//LT
			Vertices[Idx++] = (x2);
			Vertices[Idx++] = (y2);
			Vertices[Idx++] = (Color[1].x);
			Vertices[Idx++] = (Color[1].y);
			Vertices[Idx++] = (Color[1].z);
			Vertices[Idx++] = (Color[1].w);

			NewVertex(6);
		}

		public override void FillTex(float x, float y, float width, float height)
		{
			Draw(TextureColorFill, x, y, width, height);
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

			UniTexture.SetTexUnit(TextureID, 0);
			UniProjection.SetMat4(Projection);

			Vao.Bind();
			Vbo.Bind();
			Vbo.UpdateBuffer(0, Vertices);

			GL.DrawArrays(NowType, 0, (int) NumVertices);

			Program.Unbind();

			DrawCalls++;

			NumVertices = 0;
			Idx = 0;
		}

		public override void Viewport(vec4 viewport)
		{
			Viewport(viewport.x, viewport.y, viewport.z, viewport.w);
		}

		public override void Viewport(float x, float y, float w, float h)
		{
			GL.Viewport((int) x, (int) y, (int) w, (int) h);
			ViewportArray = new vec4(x, y, w, h);
		}

		public override void Scissor(PerspectiveCamera camera, vec4 viewport, float x, float y, float w, float h)
		{
			Flush();
			float x0 = camera.ToScrX(x, viewport);
			float y0 = camera.ToScrY(y, viewport);
			float x1 = camera.ToScrX(x + w, viewport);
			float y1 = camera.ToScrY(y + h, viewport);
			GL.Scissor((int) x0 - 1, (int) y0 - 1, (int) (x1 - x0 + 1), (int) (y1 - y0 + 1));
			GL.Enable(EnableCap.ScissorTest);
		}

		public override void ScissorEnd()
		{
			Flush();
			GL.Disable(EnableCap.ScissorTest);
		}

		public override void Clear()
		{
			GL.ClearColor(GLUtil.ToColor(GLDevice.Settings.ClearColor));
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public override void UseCamera(PerspectiveCamera camera)
		{
			Flush();

			Projection.ToAffine(camera.CombinedAffine);
			UniProjection.SetMat4(Projection);
		}

		//-----

		private static GLShaderProgram GetDefaultShader()
		{
			string vert = "#version 150 core\n" +
			              "\n" +
			              "in vec2 i_position;\n" +
			              "in vec4 i_color;\n" +
			              "in vec2 i_texCoord;\n" +
			              "\n" +
			              "out vec4 o_color;\n" +
			              "out vec2 o_texCoord;\n" +
			              "\n" +
			              "uniform mat4 u_proj;\n" +
			              "\n" +
			              "void main() {\n" +
			              "    o_color = i_color;\n" +
			              "    o_texCoord = i_texCoord;\n" +
			              "\n" +
			              "    gl_Position = u_proj * vec4(i_position, 0.0, 1.0);\n" +
			              "}\n";
			string frag = "#version 150 core\n" +
			              "\n" +
			              "in vec4 o_color;\n" +
			              "in vec2 o_texCoord;\n" +
			              "\n" +
			              "out vec4 fragColor;\n" +
			              "\n" +
			              "uniform sampler2D u_texture;\n" +
			              "\n" +
			              "void main() {\n" +
			              "    vec4 col = texture(u_texture, o_texCoord);\n" +
			              "    fragColor = o_color * col;\n" +
			              "}";
			return ShaderBuilds.Build(vert, frag, (program) =>
			{
				Attribute posAttrib = program.GetAttribute("i_position");
				posAttrib.Enable();
				Attribute colAttrib = program.GetAttribute("i_color");
				colAttrib.Enable();
				Attribute texAttrib = program.GetAttribute("i_texCoord");
				texAttrib.Enable();

				posAttrib.PtrFloat(2, 8, 0);
				colAttrib.PtrFloat(4, 8, 2);
				texAttrib.PtrFloat(2, 8, 6);
			});
		}

		private static GLShaderProgram GetDefaultShaderShape()
		{
			string vert = "#version 150 core\n" +
			              "\n" +
			              "in vec2 i_position;\n" +
			              "in vec4 i_color;\n" +
			              "\n" +
			              "out vec4 o_color;\n" +
			              "\n" +
			              "uniform mat4 u_proj;\n" +
			              "\n" +
			              "void main() {\n" +
			              "    o_color = i_color;\n" +
			              "\n" +
			              "    gl_Position = u_proj * vec4(i_position, 0.0, 1.0);\n" +
			              "}\n";
			string frag = "#version 150 core\n" +
			              "\n" +
			              "in vec4 o_color;\n" +
			              "\n" +
			              "out vec4 fragColor;\n" +
			              "\n" +
			              "void main() {\n" +
			              "    fragColor = o_color;\n" +
			              "}";
			return ShaderBuilds.Build(vert, frag, (program) =>
			{
				Attribute posAttrib = program.GetAttribute("i_position");
				posAttrib.Enable();
				Attribute colAttrib = program.GetAttribute("i_color");
				colAttrib.Enable();

				posAttrib.PtrFloat(2, 6, 0);
				colAttrib.PtrFloat(4, 6, 2);
			});
		}

	}

}