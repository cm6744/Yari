using System;
using Yari.Common.Registry;

namespace Yari.Grid
{

	public class TileMap<T> where T : IPalettable
	{

		public TileMapScale Suggestion;
		public byte[] Bytes;
		public Palette<T> Palette;
		public T Default;

		public TileMap(Palette<T> palette, T defval, TileMapScale suggestion = null)
		{
			Palette = palette;
			Default = defval;

			if(suggestion == null) suggestion = TileMapScale.DefSuggestion;

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

		public T Set(IPos grid, T obj)
		{
			return Set(grid.TileX, grid.TileY, grid.TileZ, obj);
		}

		public T Set(int x, int y, int z, T obj)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx + 3 >= Bytes.Length)
			{
				return Default;
			}
			T old = Palette[ReadBytes(idx)];
			WriteBytes(idx, obj.PaletteId);
			return old;
		}

		public T Get(IPos grid)
		{
			return Get(grid.TileX, grid.TileY, grid.TileZ);
		}

		public T Get(int x, int y, int z)
		{
			int idx = Index(x, y, z);
			if(idx < 0 || idx + 3 >= Bytes.Length)
			{
				return Default;
			}
			return Palette[ReadBytes(idx)];
		}

	}

	public class TileMapScale
	{

		public static TileMapScale DefSuggestion = new TileMapScale(16, 16, 2);

		public int SizeX;
		public int SizeY;
		public int SizeZ;
		public int Size;
		public int SizeInBytes;

		public TileMapScale(int x, int y, int z)
		{
			SizeX = x;
			SizeY = y;
			SizeZ = z;
			Size = SizeX * SizeY * SizeZ;
			SizeInBytes = Size * sizeof(int);
		}

	}

}