using OpenTK.Graphics.OpenGL;
using Yari.Common.Manage;

namespace Yari.Native.OpenGL
{

	public class BufferObject<T> where T : unmanaged
	{

		public int Id;
		private BufferTarget type;

		public unsafe BufferObject(T[] data, BufferTarget target)
		{
			type = target;
			Id = GL.GenBuffer();

			Bind();

			GL.BufferData(type, data.Length * sizeof(T), data, BufferUsageHint.DynamicDraw);

			Reference.FREE.OnHoldReferred(() => GL.DeleteBuffer(Id));
		}

		public void Bind()
		{
			GL.BindBuffer(type, Id);
		}

		public void Unbind()
		{
			GL.BindBuffer(type, 0);
		}

		public unsafe void UpdateBuffer(nint offset, T[] data)
		{
			Bind();

			GL.BufferSubData(type, offset, data.Length * sizeof(T), data);
		}

	}

	public class VertArrayObject<T, I>
		where T : unmanaged
		where I : unmanaged
	{

		public int Id;

		public VertArrayObject(BufferObject<T> vbo, BufferObject<I> ebo)
		{
			Id = GL.GenVertexArray();

			Bind();
			vbo.Bind();
			ebo?.Bind();

			Reference.FREE.OnHoldReferred(() => GL.DeleteVertexArray(Id));
		}

		public void Bind()
		{
			GL.BindVertexArray(Id);
		}

		public void Unbind()
		{
			GL.BindVertexArray(0);
		}

	}

}