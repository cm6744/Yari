using System.Collections.Generic;
using Yari.Common.Toolkit;

namespace Yari.Common.Manage
{

	public class Finalisation
	{

		public static Finalisation FREE = new Finalisation();

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


		private static Dictionary<Identity, Finalisation> ShortNatives = new Dictionary<Identity, Finalisation>();

		//Provides a short-time native manager, and you're supposed to free it by yourself when its usage ends.
		public static Finalisation GetFree(Identity key)
		{
			if(!ShortNatives.ContainsKey(key))
			{
				ShortNatives[key] = new Finalisation();
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