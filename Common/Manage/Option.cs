using System.Collections.Generic;
using Yari.Codec;
using Yari.Codec.General;

namespace Yari.Common.Manage
{

	public class Options
	{

		public readonly List<Option> Keys = new List<Option>();
		public BinaryCompound Values = new BinaryCompound();

		public void Join(Option option)
		{
			Keys.Add(option);
			option.Group = this;
		}

		public void WriteToLocal(FileHandler file)
		{
			BinaryIO.Write(Values, file);
		}

		public void ReadFromLocal(FileHandler file)
		{
			if(!file.Exists()) return;

			Values = BinaryIO.Read(file);
		}

	}

	public class Option
	{

		public string Key;
		public Options Group;

		public Option(string key)
		{
			Key = key;
		}

		public void Set(object o)
		{
			Group.Values.Set(Key, o);
		}

		public T Get<T>()
		{
			return Group.Values.Get<T>(Key);
		}

		public string GetString()
		{
			return Get<string>();
		}

		public double GetDouble()
		{
			return Get<double>();
		}

		public float GetFloat()
		{
			return Get<float>();
		}

		public int GetInt()
		{
			return Get<int>();
		}

		public byte GetByte()
		{
			return Get<byte>();
		}

		public bool GetBool()
		{
			return Get<bool>();
		}

		public byte[] GetBytes()
		{
			return Get<byte[]>();
		}

		public int[] GetInts()
		{
			return Get<int[]>();
		}

	}

}