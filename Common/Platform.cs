using System;
using Yari.Common.Manage;
using Yari.Common.Toolkit;
using Yari.Draw;
using Yari.Input;

namespace Yari.Common
{

	public class Platform
	{

		public static readonly Version Version = new Version("stable-1.0.0", 1);

		public static GraphicEnv GraphicEnv;
		public static InputState InputState;
		public static readonly Lifecycle Lifecycle = new Lifecycle();

		public static bool IsExited;
		public static int Ticks;
		public static int Tps, Fps;

		public static void RunStandardLifecycle(int tps, int chaseback = 1, bool syncRender = false)
		{
			float renderPartialTicks = 0f;
			long lastSyncSysClock = GraphicEnv.Nanotime;
			float tickLength = 1_000_000_000f / tps;
			int framesT = 0, framesR = 0;
			long lastCalcClock = GraphicEnv.Millitime;

			Lifecycle.TaskLoad.Invoke();

			try
			{
				while(!IsExited)
				{
					long i = GraphicEnv.Nanotime;
					float elapsedPartialTicks = (i - lastSyncSysClock) / tickLength;
					lastSyncSysClock = i;
					renderPartialTicks += elapsedPartialTicks;
					int elapsedTicks = (int) renderPartialTicks;
					renderPartialTicks -= elapsedTicks;

					for(int j = 0; j < Math.Min(chaseback, elapsedTicks); j++)
					{
						framesT++;
						Ticks++;

						InputState.StartRoll();
						Lifecycle.TaskTick.Invoke();
						InputState.EndRoll();

						if(syncRender)
						{
							Lifecycle.TaskRender.Invoke(renderPartialTicks);
							framesR++;
						}
					}

					if(!syncRender)
					{
						Lifecycle.TaskRender.Invoke(renderPartialTicks);
						framesR++;
					}

					if(GraphicEnv.Millitime - lastCalcClock >= 1000)
					{
						lastCalcClock = GraphicEnv.Millitime;
						Tps = framesT;
						Fps = framesR;
						framesT = framesR = 0;
					}
				}
			}
			catch(Exception e)
			{
				Log.Fatal(e);
			}
			Reference.FREE.OnFinalized();
		}

	}

}