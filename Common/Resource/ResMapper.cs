using System;
using System.Collections.Generic;

namespace Yari.Common.Resource
{

	public class ResMapper
	{

		private Dictionary<Type, object> ResDict = new();

		public void Load<T>(string key, T o)
		{
			Type type = typeof(T);
			
			if(!ResDict.ContainsKey(type))
			{
				ResDict[type] = new ResManager<T>();
			}

			GetManager<T>().Load(key, o);
		}

		public ResManager<T> GetManager<T>()
		{
			return ResDict[typeof(T)] as ResManager<T>;
		}

	}

}