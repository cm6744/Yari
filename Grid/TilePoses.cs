using System;
using Yari.Maths;

namespace Yari.Grid
{

	//1 Chunk = 16 Tile;
	//1 Grid = 16 Pixel;
	//1 Pixel => N Precise;
	public interface IPos
	{

		public float PreciseX { get; }
		public float PreciseY { get; }
		public int PixelX { get; }
		public int PixelY { get; }
		public int TileX { get; }
		public int TileY { get; }
		public int UnitX { get; }
		public int UnitY { get; }

		//Default impl at z = 1.
		public int TileZ => 1;

		public static int ToCoord(int tile)
		{
			return Mth.Floor(tile / 16f);
		}

	}

	public struct PrecisePos : IPos
	{

		public PrecisePos(IPos grid)
		{
			PreciseX = grid.PreciseX;
			PreciseY = grid.PreciseY;
		}

		public PrecisePos(float x, float y)
		{
			PreciseX = x;
			PreciseY = y;
		}

		public float PreciseX { get; }
		public float PreciseY { get; }
		public int PixelX => Mth.Floor(PreciseX);
		public int PixelY => Mth.Floor(PreciseY);
		public int TileX => Mth.Floor(PixelX / 16f);
		public int TileY => Mth.Floor(PixelY / 16f);
		public int UnitX => Mth.Floor(PixelX / 16f / 16f);
		public int UnitY => Mth.Floor(PixelY / 16f / 16f);

		public override bool Equals(object obj)
		{
			return obj is PrecisePos other && Equals(other);
		}

		public bool Equals(PrecisePos other)
		{
			return PreciseX.Equals(other.PreciseX) && PreciseY.Equals(other.PreciseY);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(PreciseX, PreciseY);
		}

		public static PrecisePos operator +(PrecisePos p1, PrecisePos p2)
		{
			return new PrecisePos(p1.PreciseX + p2.PreciseX, p1.PreciseY + p2.PreciseY);
		}

		public static PrecisePos operator -(PrecisePos p1, PrecisePos p2)
		{
			return new PrecisePos(p1.PreciseX - p2.PreciseX, p1.PreciseY - p2.PreciseY);
		}

		public static PrecisePos operator -(PrecisePos p)
		{
			return new PrecisePos(-p.PreciseX, -p.PreciseY);
		}

		public static bool operator ==(PrecisePos p1, PrecisePos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(PrecisePos p1, PrecisePos p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct PixPos : IPos
	{

		public PixPos(IPos grid)
		{
			PixelX = grid.PixelX;
			PixelY = grid.PixelY;
		}

		public PixPos(int x, int y)
		{
			PixelX = x;
			PixelY = y;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX { get; }
		public int PixelY { get; }
		public int TileX => Mth.Floor(PixelX / 16f);
		public int TileY => Mth.Floor(PixelY / 16f);
		public int UnitX => Mth.Floor(PixelX / 16f / 16f);
		public int UnitY => Mth.Floor(PixelY / 16f / 16f);

		public override bool Equals(object obj)
		{
			return obj is PixPos other && Equals(other);
		}

		public bool Equals(PixPos other)
		{
			return PixelX == other.PixelX && PixelY == other.PixelY;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(PixelX, PixelY);
		}

		public static PixPos operator +(PixPos p1, PixPos p2)
		{
			return new PixPos(p1.PixelX + p2.PixelX, p1.PixelY + p2.PixelY);
		}

		public static PixPos operator -(PixPos p1, PixPos p2)
		{
			return new PixPos(p1.PixelX - p2.PixelX, p1.PixelY - p2.PixelY);
		}

		public static PixPos operator -(PixPos p)
		{
			return new PixPos(-p.PixelX, -p.PixelY);
		}

		public static bool operator ==(PixPos p1, PixPos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(PixPos p1, PixPos p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct TilePos : IPos
	{

		public static int DefaultZDep = 1;

		public TilePos(IPos grid, int z)
		{
			TileX = grid.TileX;
			TileY = grid.TileY;
			TileZ = z;
		}

		public TilePos(IPos grid)
		{
			TileX = grid.TileX;
			TileY = grid.TileY;
			TileZ = grid is TilePos g2 ? g2.TileZ : DefaultZDep;
		}

		public TilePos(int x, int y, int z)
		{
			TileX = x;
			TileY = y;
			TileZ = z;
		}

		public TilePos(int x, int y)
		{
			TileX = x;
			TileY = y;
			TileZ = DefaultZDep;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX => TileX * 16;
		public int PixelY => TileY * 16;
		public int TileX { get; }
		public int TileY { get; }
		public int TileZ { get; }
		public int UnitX => Mth.Floor(TileX / 16f);
		public int UnitY => Mth.Floor(TileY / 16f);

		public override bool Equals(object obj)
		{
			return obj is TilePos other && Equals(other);
		}

		public bool Equals(TilePos other)
		{
			return TileX == other.TileX && TileY == other.TileY && TileZ == other.TileZ;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(TileX, TileY, TileZ);
		}

		public static TilePos operator +(TilePos p1, TilePos p2)
		{
			return new TilePos(p1.TileX + p2.TileX, p1.TileY + p2.TileY, p1.TileZ + p2.TileZ);
		}

		public static TilePos operator -(TilePos p1, TilePos p2)
		{
			return new TilePos(p1.TileX - p2.TileX, p1.TileY - p2.TileY, p1.TileZ - p2.TileZ);
		}

		public static TilePos operator -(TilePos p)
		{
			return new TilePos(-p.TileX, -p.TileY, -p.TileZ);
		}

		public static bool operator ==(TilePos p1, TilePos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(TilePos p1, TilePos p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct ChunkPos : IPos
	{

		public ChunkPos(IPos grid)
		{
			UnitX = grid.UnitX;
			UnitY = grid.UnitY;
		}

		public ChunkPos(int x, int y)
		{
			UnitX = x;
			UnitY = y;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX => TileX * 16;
		public int PixelY => TileY * 16;
		public int TileX => UnitX * 16;
		public int TileY => UnitY * 16;
		public int UnitX { get; }
		public int UnitY { get; }

		public override bool Equals(object obj)
		{
			return obj is ChunkPos other && Equals(other);
		}

		public bool Equals(ChunkPos other)
		{
			return UnitX == other.UnitX && UnitY == other.UnitY;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(UnitX, UnitY);
		}

		public static ChunkPos operator +(ChunkPos p1, ChunkPos p2)
		{
			return new ChunkPos(p1.UnitX + p2.UnitX, p1.UnitY + p2.UnitY);
		}

		public static ChunkPos operator -(ChunkPos p1, ChunkPos p2)
		{
			return new ChunkPos(p1.UnitX - p2.UnitX, p1.UnitY - p2.UnitY);
		}

		public static ChunkPos operator -(ChunkPos p)
		{
			return new ChunkPos(-p.UnitX, -p.UnitY);
		}

		public static bool operator ==(ChunkPos p1, ChunkPos p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(ChunkPos p1, ChunkPos p2)
		{
			return !p1.Equals(p2);
		}

	}

}