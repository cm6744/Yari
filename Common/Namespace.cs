using Yari.Common.Resource;

namespace Yari.Common
{

	public class Namespace
	{

		public string Name;
		public Localer Localer = new Localer();
		public ResMapper ResMapper = new ResMapper();

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