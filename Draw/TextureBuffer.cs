﻿using System;

namespace Yari.Draw
{

	public interface TextureBuffer
	{

		public Texture Src { get; }

		void Begin(DrawBatch batch);

		void End(DrawBatch batch);

		void Free();

	}

}