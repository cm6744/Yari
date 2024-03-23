namespace Yari.Common
{

	public class Namespace
	{

		public string Name;
		public YariCoreI18n I18n = new YariCoreI18n();
		public YariCoreResources Resources = new YariCoreResources();

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