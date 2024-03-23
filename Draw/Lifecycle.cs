namespace Yari.Draw
{

	public delegate void RunRender(float partialTicks);
	public delegate void RunTick();
	public delegate void Resize(int ow, int oh, int w, int h);
	public delegate void Load();

	public class Lifecycle
	{

		public Load TaskLoad = () => { };
		public RunRender TaskRender = (f) => { };
		public RunTick TaskTick = () => { };
		public Resize TaskResize = (ow1, oh1, w1, w2) => { };

		public float Fps, Tps;

	}

}