using System.Collections.Generic;
using Yari.Codec.General;

namespace Yari.Codec.Passle
{

	public class PassleIO
	{

		public static BinaryCompound Read(FileHandler handler)
		{
			string code = StringIO.GetCompression(StringIO.ReadArray(handler),
				s => !s.StartsWith("#") && !s.StartsWith("//"));
			return ReadRaw(code);
		}

		//Not able to deal with marks.
		public static BinaryCompound ReadRaw(string code)
		{
			Passle240 ps = new Passle240(code);
			BinaryCompound compound = (BinaryCompound) ps.Parse();
			GenerateFastAccesses(compound);
			return compound;
		}

		private static BinaryCompound mergingTemp;

		public static void GenerateFastAccesses(BinaryCompound compound)
		{
			mergingTemp = new BinaryCompound();
			GenerateFastAccesses("", compound);
			compound.Merge(mergingTemp);
			mergingTemp = null;
		}

		public static void GenerateFastAccesses(string address, BinaryCompound compound)
		{
			foreach(KeyValuePair<string, object> entry in compound.Map)
			{
				object v = entry.Value;
				string k = entry.Key;
				string newAddress = address;

				if(string.IsNullOrEmpty(newAddress))
				{
					newAddress = k;
				}
				else
				{
					newAddress += "." + k;
				}

				if(v is BinaryCompound compound1)
				{
					GenerateFastAccesses(newAddress, compound1);
					continue;
				}

				mergingTemp.Set(newAddress, v);
			}
		}

	}

}