﻿namespace Yari.Common
{

	public delegate void RunRender(float partial);

	public delegate void RunTick();

	public delegate void Resize(int ow, int oh, int w, int h);

	public delegate void Load();

	public class Lifecycle
	{

		public Load TaskLoad = () => { };
		public RunRender TaskRender = (partial) => { };
		public RunTick TaskTick = () => { };
		public Resize TaskResize = (ow1, oh1, w1, w2) => { };

	}

}