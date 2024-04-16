using Yari.Maths.Structs;

namespace Yari.Draw
{

	public interface GraphicEnvironment
	{

		string Title { set; }

		vec2 Size { get; set; }

		vec2 Pos { get; set; }

		bool Decorated { set; }

		long Nanotime { get; }

		long Millitime => Nanotime / 1000_000;

		void Swap();

		void Prepare();

	}

}