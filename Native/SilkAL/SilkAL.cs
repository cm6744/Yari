using Silk.NET.OpenAL;
using Yari.Audio;
using Yari.Common;

namespace Yari.Native.SilkAL
{

	public class SilkAL
	{

		public static ALContext ALC;
		public static AL AL;

		public static unsafe void InitALDevice()
		{
			ALC = ALContext.GetApi();
			AL = AL.GetApi();

			Device* device = ALC.OpenDevice("");
			Context* ctx = ALC.CreateContext(device, null);
			ALC.MakeContextCurrent(ctx);

			Platform.Lifecycle.TaskTick += AudioClip.CheckClipStates;
		}

	}

}