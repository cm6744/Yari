﻿using System.Collections.Generic;

namespace Yari.Common.Registry
{

	public class Palette<V>
	{

		private Dictionary<int, V> map0 = new Dictionary<int, V>();
		private List<V> list0 = new List<V>();
		private Dictionary<V, int> map1 = new Dictionary<V, int>();
		private int idCount;

		public void Add(V v)
		{
			map0[idCount] = v;
			list0.Add(v);
			map1[v] = idCount;

			idCount++;
		}

		public V Get(int id)
		{
			return list0[id];
		}

		public int IdFor(V v)
		{
			return map1.GetValueOrDefault(v, 0);
		}

		public Dictionary<int, V> Mapped()
		{
			return map0;
		}

		public V this[int index]
		{
			get => Get(index);
		}

	}

}