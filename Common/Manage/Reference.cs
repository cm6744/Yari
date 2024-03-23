using System.Collections.Generic;
using Yari.Common.Toolkit;

namespace Yari.Common.Manage
{

	public class Reference
	{

		public static Reference FREE = new Reference();

		private List<Runnable> ToRelease = new List<Runnable>();

		public void OnHoldReferred(Runnable release)
		{
			ToRelease.Add(release);
		}

		public void OnFinalized()
		{
			ToRelease.ForEach(r => r.Invoke());
			ToRelease.Clear();

			if(this == FREE)
			{
				Log.Info("Final disposing ended. GLOBAL-FREE was cleared.");
			}
		}


		private static Dictionary<Identity, Reference> ShortNatives = new Dictionary<Identity, Reference>();

		//Provides a short-time native manager, and you're supposed to free it by yourself when its usage ends.
		public static Reference GetFree(Identity key)
		{
			if(!ShortNatives.ContainsKey(key))
			{
				ShortNatives[key] = new Reference();
			}

			return ShortNatives[key];
		}

		public static void Free(Identity key)
		{
			ShortNatives[key]?.OnFinalized();
			ShortNatives.Remove(key);
		}

	}

}