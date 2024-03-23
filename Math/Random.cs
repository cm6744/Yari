using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = System.Random;

namespace Yari.Math
{

	public abstract class Rand
	{

		public bool Next()
		{
			return NextFloat() <= 0.5f;
		}

		public abstract float NextFloat();

		public abstract int NextInt(int bound);

		public float NextFloat(float min, float max)
		{
			return NextFloat() * (max - min) + min;
		}

		public int NextInt(int min, int max)
		{
			max++;
			return NextInt(max - min) + min;
		}

		public abstract void SetSeed(long seed);

		public abstract long GetISeed();

		public abstract Rand Copy();

		public abstract Rand Copyx(int offset);

		public T Select<T>(Collection<T> col)
		{
			if(col == null)
			{
				return default;
			}
			if(col.Count == 0)
			{
				return default;
			}

			return col[NextInt(col.Count)];
		}

		public T Select<T>(params T[] arr)
		{
			if(arr == null)
			{
				return default;
			}
			if(arr.Length == 0)
			{
				return default;
			}

			return arr[NextInt(arr.Length)];
		}

		public float NextNDFloat()
		{
			float x, y, w;
			do
			{
				x = NextFloat() * 2 - 1;
				y = NextFloat() * 2 - 1;
				w = x * x + y * y;
			}
			while(w >= 1 || w == 0);
			double c = Mth.Sqrt(-2 * Mth.Log(w) / w);
			return (float) (y * c);//Use a temp is good but this is fast enough.
		}

		public int NextNDInt()
		{
			return Mth.Round(NextNDFloat());
		}

	}

	public class RandMac : Rand
	{

		public long InitialSeed;
		public long NowSeed;
		int M = 197;
		int A = 484;
		int C = 103;

		public RandMac(long initialSeed)
		{
			SetSeed(initialSeed);
		}

		public RandMac()
		{
			SetSeed(new Random().Next());
		}

		long NewSeed()
		{
			long gen = 1;

			do
			{
				gen = (M * NowSeed * gen + A) % C;
				NowSeed = (NowSeed * M + A) >> C - A;
			}
			while(gen < -100 || gen > 100);

			return gen;
		}

		public override float NextFloat()
		{
			long i = Mth.Abs(NewSeed());
			return i * 0.01F;
		}

		public override int NextInt(int bound)
		{
			return (int) (bound * NextFloat());
		}

		public override void SetSeed(long seed)
		{
			InitialSeed = seed;
			NowSeed = seed;
		}

		public override long GetISeed()
		{
			return InitialSeed;
		}

		public override Rand Copy()
		{
			return new RandMac(InitialSeed);
		}

		public override Rand Copyx(int offset)
		{
			return new RandMac(InitialSeed + offset);
		}

	}

	public class RandCsp : Rand
	{

		private long Seed0;
		private Random CsRandom;

		public RandCsp(long initialSeed)
		{
			SetSeed(initialSeed);
		}

		public RandCsp()
		{
			SetSeed(new Random().Next());
		}

		public override float NextFloat()
		{
			return (float) CsRandom.NextDouble();
		}

		public override int NextInt(int bound)
		{
			return CsRandom.Next(bound);
		}

		public override void SetSeed(long seed)
		{
			Seed0 = seed;
			CsRandom = new Random((int) seed);
		}

		public override long GetISeed()
		{
			return Seed0;
		}

		public override Rand Copy()
		{
			return new RandCsp(Seed0);
		}

		public override Rand Copyx(int offset)
		{
			return new RandCsp(Seed0 + offset);
		}

	}

}
