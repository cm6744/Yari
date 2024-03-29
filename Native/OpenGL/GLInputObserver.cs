using Yari.Input;

namespace Yari.Native.OpenGL
{

	public class GLInputObserver : InputObserver
	{

		public static long InputCheckTicks;

		long PressOccur;
		int Press;

		public void Fire()
		{
			Press = 1;
			PressOccur = InputCheckTicks;
		}

		public void Consume()
		{
			Press = 0;
			PressOccur = -1;
		}

		public int HoldTime()
		{
			if(Press != 1)
			{
				return 0;
			}

			return (int) (InputCheckTicks - PressOccur);
		}

		public bool Pressed()
		{
			return Press == 1 && PressOccur == InputCheckTicks;
		}

		public bool Holding()
		{
			return Press == 1;
		}

	}

}