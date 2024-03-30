using Yari.Common;

namespace Yari.Draw.Gui
{

	public class Resolution
	{

		public float ScaledWidth;
		public float ScaledHeight;
		public float ScaleFactor;

		public Resolution(Screen curScreen, float scaleForced = 0, bool limitInt = false)
		{
			ScaledWidth = Platform.GraphicEnv.Size.x;
			ScaledHeight = Platform.GraphicEnv.Size.y;

			if(scaleForced <= 0)
			{
				ScaleFactor = 0.5f;

				while(ScaleFactor < 1.5f
				      && ScaledWidth / (ScaleFactor + 1) >= 320
				      && ScaledHeight / (ScaleFactor + 1) >= 240)
				{
					ScaleFactor += 0.5f;
				}

				if(limitInt && (ScaleFactor * 2) % 2 != 0 && (ScaleFactor - 0.5f) > 0)
				{
					ScaleFactor -= 0.5f;
				}
			}
			else
			{
				ScaleFactor = scaleForced;
			}

			if(curScreen != null)
			{
				ScaleFactor *= curScreen.ScaleMul;

				float locked = curScreen.ScaleLocked;

				if(locked > 0)
				{
					ScaleFactor = locked;
				}
			}

			ScaledWidth /= ScaleFactor;
			ScaledHeight /= ScaleFactor;
		}

	}

}