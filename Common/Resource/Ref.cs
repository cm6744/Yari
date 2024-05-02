﻿namespace Yari.Common.Resource
{

	public struct Ref<T>
	{

		public T Value => CheckedGet();
		public bool IsPresent => Value != null;

		private T ValueOptional;
		public string Key;
		private IdentityManager<T> Getter;

		public Ref(IdentityManager<T> resources, string key)
		{
			Getter = resources;
			Key = key;
			ValueOptional = default(T);
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