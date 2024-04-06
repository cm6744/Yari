using System;
using Yari.Common.Registry;

namespace Yari.Grid
{

	public class GridBuffer<T> where T : IPalettable
	{

		public GridBufferSuggestion Suggestion;
		public byte[] Bytes;
		public Palette<T> Palette;
		public T Default;

		public GridBuffer(Palette<T> palette, T defval, GridBufferSuggestion suggestion = null)
		{
			Palette = palette;
			Default = defval;

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

		public T Set(Grid grid, T obj)
		{
			return Set(grid.GridX, grid.GridY, grid.GridZ, obj);
		}

		public T Set(IGrid grid, int z, T obj)
		{
			return Set(grid.GridX, grid.GridY, z, obj);
		}

		public T Set(int x, int y, int z, T obj)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx >= Bytes.Length)
			{
				return Default;
			}
			T old = Palette[ReadBytes(idx)];
			WriteBytes(idx, obj.PaletteId);
			return old;
		}

		public T Get(Grid grid)
		{
			return Get(grid.GridX, grid.GridY, grid.GridZ);
		}

		public T Get(IGrid grid, int z)
		{
			return Get(grid.GridX, grid.GridY, z);
		}

		public T Get(int x, int y, int z)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx >= Bytes.Length)
			{
				return Default;
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