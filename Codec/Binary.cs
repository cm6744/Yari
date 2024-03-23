using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Yari.Codec.General;

namespace Yari.Codec
{

	public interface IBinary
	{
	}

	public class BinaryCompound : IBinary
	{

		//The map might contains some non-primitive values.
		public Dictionary<string, object> Map;
		public readonly bool IsReadOnly;

		public BinaryCompound(Dictionary<string, object> MapCpy, bool isReadOnly = false)
		{
			Map = MapCpy;
			IsReadOnly = isReadOnly;
		}

		public BinaryCompound(bool isReadOnly = false)
		{
			Map = new Dictionary<string, object>();
			IsReadOnly = isReadOnly;
		}

		public void Clear()
		{
			if(IsReadOnly)
			{
				return;
			}
			Map.Clear();
		}

		public void Merge(BinaryCompound compound)
		{
			if(IsReadOnly)
			{
				return;
			}
			Map = Map.Concat(compound.Map).ToDictionary(k => k.Key, v => v.Value);
		}

		public T Get<T>(string key)
		{
			return (T) Map[key];
		}

		public string GetString(string key)
		{
			return Get<string>(key);
		}

		public double GetDouble(string key)
		{
			return Get<double>(key);
		}

		public float GetFloat(string key)
		{
			return Get<float>(key);
		}

		public int GetInt(string key)
		{
			return Get<int>(key);
		}

		public byte GetByte(string key)
		{
			return Get<byte>(key);
		}

		public bool GetBool(string key)
		{
			return Get<bool>(key);
		}

		public byte[] GetBytes(string key)
		{
			return Get<byte[]>(key);
		}

		public BinaryCompound GetCompound(string key)
		{
			return Get<BinaryCompound>(key);
		}

		public BinaryList GetList(string key)
		{
			return Get<BinaryList>(key);
		}

		public void Set(string key, object v)
		{
			if(IsReadOnly)
			{
				return;
			}
			Map[key] = v;
		}

	}

	public class BinaryList : IBinary, IEnumerable<object>
	{

		public List<object> List;

		public byte Type
		{
			get
			{
				if(List == null || List.Count == 0)
				{
					return 0;
				}

				return BinaryIO.GetId(List[0]);
			}
		}
		public int Count => List.Count;

		public BinaryList(List<object> ListCpy)
		{
			List = ListCpy;
		}

		public BinaryList()
		{
			List = new List<object>();
		}

		public object this[int index]
		{
			get => Get<object>(index);
			set => Set(index, value);
		}

		public T Get<T>(int i)
		{
			return (T) List[i];
		}

		public void Add(object v)
		{
			List.Add(v);
		}

		public void Set(int i, object v)
		{
			List[i] = v;
		}

		public IEnumerator<object> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}

}
