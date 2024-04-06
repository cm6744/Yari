using System;

namespace Yari.Common
{

	public class Version : IComparable<Version>
	{

		public string FullName;
		public string Prefix;
		public bool Snapshot;
		public bool Stable;
		public int Iteration;

		public Version(string code, int iteration)
		{
			FullName = code;
			Prefix = code[code.IndexOf('-')..];
			Snapshot = Prefix.Contains("snapshot", StringComparison.OrdinalIgnoreCase);
			Stable = !Snapshot;

			Iteration = iteration;
		}

		public int CompareTo(Version other)
		{
			if(other.Iteration < Iteration) return 1;
			if(other.Iteration == Iteration) return 0;
			return -1;
		}

	}

}