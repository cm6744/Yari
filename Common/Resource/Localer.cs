using System.Collections.Generic;
using Yari.Codec;

namespace Yari.Common.Resource
{

	public class Localer
	{

		public Dictionary<string, BinaryCompound> Langs = new();
		public string LangKey = "EN_US";

		public void Load(string key, BinaryCompound compound)
		{
			Langs[key] = compound;
		}

		public string Get(string key)
		{
			return Langs[LangKey].GetString(key);
		}

	}

}