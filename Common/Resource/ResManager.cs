﻿using System.Collections.Generic;
using Yari.Audio;
using Yari.Codec;
using Yari.Draw;

namespace Yari.Common.Resource
{

	public class ResManager<T>
	{

		private Dictionary<string, T> ResDict = new();

		public void Load(string key, T o)
		{
			ResDict[key] = o;
		}

		public void Unload(string key)
		{
			ResDict.Remove(key);
		}

		public T Get(string key)
		{
			return ResDict[key];
		}

		public Ref<T> Refer(string key)
		{
			return new Ref<T>(this, key);
		}

	}

}