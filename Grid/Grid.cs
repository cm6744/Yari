using System;
using Yari.Maths;

namespace Yari.Grid
{

	//1 Chunk = 16 Grid;
	//1 Grid = 16 Pixel;
	//1 Pixel => N Precise;
	public interface IGrid
	{

		public float PreciseX { get; }
		public float PreciseY { get; }
		public int PixelX { get; }
		public int PixelY { get; }
		public int GridX { get; }
		public int GridY { get; }
		public int ChunkX { get; }
		public int ChunkY { get; }

	}

	public struct Precise : IGrid
	{

		public Precise(IGrid grid)
		{
			PreciseX = grid.PreciseX;
			PreciseY = grid.PreciseY;
		}

		public Precise(float x, float y)
		{
			PreciseX = x;
			PreciseY = y;
		}

		public float PreciseX { get; }
		public float PreciseY { get; }
		public int PixelX => Mth.Floor(PreciseX);
		public int PixelY => Mth.Floor(PreciseY);
		public int GridX => Mth.Floor(PixelX / 16f);
		public int GridY => Mth.Floor(PixelY / 16f);
		public int ChunkX => Mth.Floor(PixelX / 16f / 16f);
		public int ChunkY => Mth.Floor(PixelY / 16f / 16f);

		public override bool Equals(object obj)
		{
			return obj is Precise other && Equals(other);
		}

		public bool Equals(Precise other)
		{
			return PreciseX.Equals(other.PreciseX) && PreciseY.Equals(other.PreciseY);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(PreciseX, PreciseY);
		}

		public static Precise operator +(Precise p1, Precise p2)
		{
			return new Precise(p1.PreciseX + p2.PreciseX, p1.PreciseY + p2.PreciseY);
		}

		public static Precise operator -(Precise p1, Precise p2)
		{
			return new Precise(p1.PreciseX - p2.PreciseX, p1.PreciseY - p2.PreciseY);
		}

		public static Precise operator -(Precise p)
		{
			return new Precise(-p.PreciseX, -p.PreciseY);
		}

		public static bool operator ==(Precise p1, Precise p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(Precise p1, Precise p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct Pixel : IGrid
	{

		public Pixel(IGrid grid)
		{
			PixelX = grid.PixelX;
			PixelY = grid.PixelY;
		}

		public Pixel(int x, int y)
		{
			PixelX = x;
			PixelY = y;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX { get; }
		public int PixelY { get; }
		public int GridX => Mth.Floor(PixelX / 16f);
		public int GridY => Mth.Floor(PixelY / 16f);
		public int ChunkX => Mth.Floor(PixelX / 16f / 16f);
		public int ChunkY => Mth.Floor(PixelY / 16f / 16f);

		public override bool Equals(object obj)
		{
			return obj is Pixel other && Equals(other);
		}

		public bool Equals(Pixel other)
		{
			return PixelX == other.PixelX && PixelY == other.PixelY;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(PixelX, PixelY);
		}

		public static Pixel operator +(Pixel p1, Pixel p2)
		{
			return new Pixel(p1.PixelX + p2.PixelX, p1.PixelY + p2.PixelY);
		}

		public static Pixel operator -(Pixel p1, Pixel p2)
		{
			return new Pixel(p1.PixelX - p2.PixelX, p1.PixelY - p2.PixelY);
		}

		public static Pixel operator -(Pixel p)
		{
			return new Pixel(-p.PixelX, -p.PixelY);
		}

		public static bool operator ==(Pixel p1, Pixel p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(Pixel p1, Pixel p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct Grid : IGrid
	{

		public static int DefaultZDep = 0;

		public Grid(IGrid grid)
		{
			GridX = grid.GridX;
			GridY = grid.GridY;
			GridZ = grid is Grid g2 ? g2.GridZ : DefaultZDep;
		}

		public Grid(int x, int y, int z)
		{
			GridX = x;
			GridY = y;
			GridZ = z;
		}

		public Grid(int x, int y)
		{
			GridX = x;
			GridY = y;
			GridZ = DefaultZDep;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX => GridX * 16;
		public int PixelY => GridY * 16;
		public int GridX { get; }
		public int GridY { get; }
		public int GridZ { get; }
		public int ChunkX => Mth.Floor(PixelX / 16f / 16f);
		public int ChunkY => Mth.Floor(PixelY / 16f / 16f);

		public override bool Equals(object obj)
		{
			return obj is Grid other && Equals(other);
		}

		public bool Equals(Grid other)
		{
			return GridX == other.GridX && GridY == other.GridY && GridZ == other.GridZ;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GridX, GridY, GridZ);
		}

		public static Grid operator +(Grid p1, Grid p2)
		{
			return new Grid(p1.GridX + p2.GridX, p1.GridY + p2.GridY, p1.GridZ + p2.GridZ);
		}

		public static Grid operator -(Grid p1, Grid p2)
		{
			return new Grid(p1.GridX - p2.GridX, p1.GridY - p2.GridY, p1.GridZ - p2.GridZ);
		}

		public static Grid operator -(Grid p)
		{
			return new Grid(-p.GridX, -p.GridY, -p.GridZ);
		}

		public static bool operator ==(Grid p1, Grid p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(Grid p1, Grid p2)
		{
			return !p1.Equals(p2);
		}

	}

	public struct ChunkGrid : IGrid
	{

		public ChunkGrid(IGrid grid)
		{
			ChunkX = grid.ChunkX;
			ChunkY = grid.ChunkY;
		}

		public ChunkGrid(int x, int y)
		{
			ChunkX = x;
			ChunkY = y;
		}

		public float PreciseX => PixelX;
		public float PreciseY => PixelY;
		public int PixelX => GridX * 16;
		public int PixelY => GridY * 16;
		public int GridX => ChunkX * 16;
		public int GridY => ChunkY * 16;
		public int ChunkX { get; }
		public int ChunkY { get; }

		public override bool Equals(object obj)
		{
			return obj is ChunkGrid other && Equals(other);
		}

		public bool Equals(ChunkGrid other)
		{
			return ChunkX == other.ChunkX && ChunkY == other.ChunkY;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ChunkX, ChunkY);
		}

		public static ChunkGrid operator +(ChunkGrid p1, ChunkGrid p2)
		{
			return new ChunkGrid(p1.ChunkX + p2.ChunkX, p1.ChunkY + p2.ChunkY);
		}

		public static ChunkGrid operator -(ChunkGrid p1, ChunkGrid p2)
		{
			return new ChunkGrid(p1.ChunkX - p2.ChunkX, p1.ChunkY - p2.ChunkY);
		}

		public static ChunkGrid operator -(ChunkGrid p)
		{
			return new ChunkGrid(-p.ChunkX, -p.ChunkY);
		}

		public static bool operator ==(ChunkGrid p1, ChunkGrid p2)
		{
			return p1.Equals(p2);
		}

		public static bool operator !=(ChunkGrid p1, ChunkGrid p2)
		{
			return !p1.Equals(p2);
		}

	}

}