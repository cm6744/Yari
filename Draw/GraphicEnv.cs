using Yari.Maths.Structs;

namespace Yari.Draw
{

	public interface GraphicEnvironment
	{

		string Title { set; }

		vec2 Size { get; set; }

		vec2 Pos { get; set; }

		bool Decorated { set; }

		double Nanotime { get; }

		long Millitime => (long) (Nanotime / 1000_000);

		void Swap();

		void Prepare();

	}

}