using System;
using System.Collections.Generic;
using Yari.Codec;
using Yari.Common.Registry;
using Yari.Common.Toolkit;

namespace Yari.Common.Resource
{

	public class AssetMapper
	{
		
		//STATIC

		static readonly Dictionary<Namespace, AssetMapper> Mappers = new Dictionary<Namespace, AssetMapper>();

		public static AssetMapper From(Namespace nsp)
		{
			return !Mappers.ContainsKey(nsp) ? null : Mappers[nsp];
		}

		//END

		private Dictionary<Type, object> ResDict = new Dictionary<Type, object>();

		public AssetMapper(Namespace nsp)
		{
			if(Mappers.ContainsKey(nsp))
			{
				Log.Warn("A Namespace shouldn't create AssetMapper over 1 time.");
				return;
			}
			Mappers[nsp] = this;
		}
		
		public void Load<T>(string key, T o)
		{
			GetManager<T>().Load(key, o);
		}

		public AssetManager<T> GetManager<T>()
		{
			Type type = typeof(T);

			if(!ResDict.ContainsKey(type))
			{
				ResDict[type] = new AssetManager<T>();
			}

			return ResDict[typeof(T)] as AssetManager<T>;
		}

	}

}