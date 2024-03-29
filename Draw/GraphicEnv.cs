using Yari.Maths;

namespace Yari.Draw
{

	public interface GraphicEnv
	{

		vec2 Size { get; }
		vec2 Pos { get; }
		long Nanotime { get; }
		long Millitime => Nanotime / 1000_000;

	}

}