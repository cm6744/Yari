using Box2D.NetStandard.Dynamics.Fixtures;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.Mime.MediaTypeNames;
using System;
using Yari.Common.Manage;
using Yari.Common;
using Yari.Draw;
using Yari.Draw.Extended;
using Yari.Input;
using Yari.Math;

namespace Yari.Gui.Element
{

	public class ElementButton : Element
	{

		public static InputObserver LEFT_CODE = InputState.Instance.Observe(Keycode.BUTTON_LEFT);
		public static InputObserver RIGHT_CODE = InputState.Instance.Observe(Keycode.BUTTON_RIGHT);

		public static int DEFAULT_PRESS_DELAY = 2;

		public Icon[] Icons = new Icon[3];
		public Texture Texture3Line;
		public string Text;

		public Runnable OnLeftFired = () => {};
		public Runnable OnRightFired = () => {};

		private int pressDelay;
		private bool cursorOn;

		public new void Input(InputState input, vec2 cursor)
		{
			cursorOn = false;

			if(Bound.Contains(cursor))
			{
				cursorOn = true;

				if(LEFT_CODE.Pressed())
				{
					OnLeftFired.Invoke();
					pressDelay = DEFAULT_PRESS_DELAY;
				}
				if(RIGHT_CODE.Pressed())
				{
					OnRightFired.Invoke();
					pressDelay = DEFAULT_PRESS_DELAY;
				}
			}
		}

		public new void Update()
		{
			pressDelay--;
		}

		public void onRender(DrawBatch batch)
		{
			if(Texture3Line == null)
			{
				if(pressDelay > 0)
				{
					batch.Draw(Icons[2], Bound);
				}
				else if(cursorOn)
				{
					batch.Draw(Icons[1], Bound);
				}
				else
				{
					batch.Draw(Icons[0], Bound);
				}

				batch.Draw(Text, Bound.x, Bound.yp + 4, Align.CENTER);
			}
			else
			{
				float sy;
				if(pressDelay > 0)
				{
					sy = Texture3Line.Height / 3f * 2;
				}
				else if(cursorOn)
				{
					sy = Texture3Line.Height / 3f;	
				}
				else
				{
					sy = 0;
				}
				batch.Draw(Texture3Line, Bound, 0, sy, Texture3Line.Width, Texture3Line.Height / 3f);
			}
		}

	}

}