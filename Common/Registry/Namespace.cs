using Yari.Common.Resource;

namespace Yari.Common.Registry
{

	public class Namespace
	{

		public string Name;

		public Namespace(string name)
		{
			Name = name;
		}

		public Identity Identity(string key)
		{
			return new Identity(Name, key);
		}

	}

}