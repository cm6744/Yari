namespace Yari.Common.Registry
{

	public struct Identity
	{

		public const int NullId = -1;

		public static string DefaultNamespace;

		public string Namespace;
		public string Key;
		public int Id;

		public Identity(string nps, string key, int id = NullId)
		{
			Namespace = nps;
			Key = key;
			Id = id;
		}

		public Identity(string path, int id = NullId)
		{
			if(!path.Contains(':'))
			{
				Namespace = DefaultNamespace;
				Key = path;
			}
			else
			{
				string[] arr = path.Split(':');
				Namespace = arr[0];
				Key = arr[1];
			}
			Id = id;
		}

		public bool Insideof(string path)
		{
			return Key.StartsWith(path);
		}

		public override bool Equals(object obj)
		{
			if(obj is not Identity identity)
			{
				return false;
			}

			return this == identity;
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public static bool operator ==(Identity i1, Identity i2)
		{
			if(i1.Id == NullId || i2.Id == NullId)
			{
				return i1.Key == i2.Key && i1.Namespace == i2.Namespace;
			}

			return i1.Id == i2.Id;
		}

		public static bool operator !=(Identity i1, Identity i2)
		{
			return !(i1 == i2);
		}

		public string Path => ToString();

		public override string ToString()
		{
			return Namespace + ":" + Key;
		}

	}

}