namespace Yari.Common.Resource
{

	public class Ref<T>
	{

		public T Value => CheckedGet();
		public bool IsPresent => Value != null;

		private T ValueOptional;
		public string Key;
		private ResManager<T> Getter;

		public Ref(ResManager<T> resources, string key)
		{
			Getter = resources;
			Key = key;
		}

		private T CheckedGet()
		{
			if(ValueOptional != null)
			{
				return ValueOptional;
			}
			return ValueOptional = Getter.Get(Key);
		}

	}

}