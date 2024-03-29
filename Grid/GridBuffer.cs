using System;
using Yari.Common;

namespace Yari.Grid
{

	public class GridBuffer<T> where T : Identifiable
	{

		public GridBufferSuggestion Suggestion;
		public byte[] Bytes;
		public IdentityRegister<T> Palette;

		public GridBuffer(IdentityRegister<T> register, GridBufferSuggestion suggestion = null)
		{
			Palette = register;

			if(suggestion == null) suggestion = GridBufferSuggestion.DefSuggestion;

			Suggestion = suggestion;
			Bytes = new byte[Suggestion.SizeInBytes];
		}

		public int Index(int x, int y, int z)
		{
			int prx = Suggestion.SizeX - 1;
			int pry = Suggestion.SizeY - 1;
			x &= prx;
			y &= pry;

			return ((x * Suggestion.SizeY + y) * Suggestion.SizeZ + z) * sizeof(int);
		}

		private void WriteBytes(int idx, int v)
		{
			for(int i = 0; i < 4; i++)
			{
				Bytes[idx + i] = (byte) (v >> (i << 3) & 0xff);
			}
		}

		private int ReadBytes(int idx)
		{
			int ri = 0;

			for(int i = 0; i < 4; i++)
			{
				ri <<= 8;
				ri |= Bytes[idx + 3 - i];
			}

			return ri;
		}

		public void Set(Grid grid, T obj)
		{
			Set(grid.GridX, grid.GridY, grid.GridZ, obj);
		}

		public void Set(int x, int y, int z, T obj)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx >= Bytes.Length)
			{
				return;
			}
			WriteBytes(idx, obj.Registry.Id);
		}

		public T Get(Grid grid)
		{
			return Get(grid.GridX, grid.GridY, grid.GridZ);
		}

		public T Get(int x, int y, int z)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx >= Bytes.Length)
			{
				return Palette.DefaultValue;
			}
			return Palette[ReadBytes(idx)];
		}

	}

	public class GridBufferSuggestion
	{

		public static GridBufferSuggestion DefSuggestion = new GridBufferSuggestion(16, 16, 2);

		public int SizeX;
		public int SizeY;
		public int SizeZ;
		public int Size;
		public int SizeInBytes;

		public GridBufferSuggestion(int x, int y, int z)
		{
			SizeX = x;
			SizeY = y;
			SizeZ = z;
			Size = SizeX * SizeY * SizeZ;
			SizeInBytes = Size * sizeof(int);
		}

	}

}